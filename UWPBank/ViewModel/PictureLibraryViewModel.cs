using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPBank.ViewModel
{
    //Windows 10 - Accelerate File Operations with the Search Indexer
    //https://msdn.microsoft.com/en-us/magazine/mt620012.aspx?f=255&MSPPError=-2147217396
    public class PictureLibraryViewModel
    {
        private ObservableCollection<PictureFileInfo> _allPictures;
        private CancellationTokenSource _cts; //to cancel lengthy process of loading picture

        public PictureLibraryViewModel()
        {
            _allPictures = new ObservableCollection<PictureFileInfo>();
        }

        public ObservableCollection<PictureFileInfo> AllPictures
        {
            get
            {
                return _allPictures;
            }
        }

        public async Task UpdatePictures()
        {
            try
            {
                _cts = new CancellationTokenSource();
                await UpdatePicturesUsingIndexer(_cts.Token);
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Task cancelled in UpdatePictures().");
            }
        }

        public void CancelUpdatingPictures()
        {
            if (_cts != null && _cts.Token.CanBeCanceled)
                _cts.Cancel();
        }

        private async Task UpdatePicturesUsingIndexer(CancellationToken cancellationToken)
        {
            Stopwatch watch = Stopwatch.StartNew();

            _allPictures.Clear(); //clear the result of previous operation

            QueryOptions options = new QueryOptions(CommonFileQuery.OrderByDate,
                new String[] { ".jpg", ".jpeg", ".png" })
            {
                IndexerOption = IndexerOption.UseIndexerWhenAvailable
            };
            options.SetPropertyPrefetch(PropertyPrefetchOptions.None,
                new String[] { "System.Size", "System.DateModified" });

            // Set up thumbnail prefetch.
            const uint requestedSize = 190;
            const ThumbnailMode thumbnailMode = ThumbnailMode.PicturesView;
            const ThumbnailOptions thumbnailOptions = ThumbnailOptions.UseCurrentScale;
            options.SetThumbnailPrefetch(thumbnailMode, requestedSize, thumbnailOptions);

            StorageFileQueryResult queryResult = KnownFolders.PicturesLibrary.CreateFileQueryWithOptions(options);

            uint index = 0, stepSize = 10;
            IReadOnlyList<StorageFile> files = await queryResult.GetFilesAsync(index, stepSize);

            // Note that I'm paging in the files as described
            while (files.Count != 0)
            {
                foreach (StorageFile file in files)
                {
                    IDictionary<string, object> props =
                      await file.Properties.RetrievePropertiesAsync(
                      new String[] { "System.Size", "System.DateModified" });

                    var thumbnail = await file.GetThumbnailAsync(thumbnailMode, requestedSize, thumbnailOptions);

                    _allPictures.Add(new PictureFileInfo()
                    {
                        FileName = file.Name,
                        FileSize = (ulong)props["System.Size"],
                        DateModified = (DateTimeOffset)props["System.DateModified"],
                        Thumbnail = thumbnail
                    });
                }

                //If a cancellation is requested, abort the operation.
                cancellationToken.ThrowIfCancellationRequested();

                index += 10;
                var fileTask = queryResult.GetFilesAsync(index, stepSize).AsTask();
                files = await fileTask;                
            }
            Debug.WriteLine($"{_allPictures.Count} pictures are listed. Elapsed ms: {watch.ElapsedMilliseconds}");
        }
    }

    public class PictureFileInfo
    {
        public string FileName { get; set; }
        public ulong FileSize { get; set; }
        public DateTimeOffset DateModified { get; set; }
        public StorageItemThumbnail Thumbnail { get; set; }
    }

    public class ThumbnailImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is StorageItemThumbnail)
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(value as StorageItemThumbnail);
                return bitmapImage;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class DateTimeOffsetFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value.GetType() == typeof(System.DateTimeOffset))
            {
                return ((System.DateTimeOffset)value).LocalDateTime.ToString("yyyy/MM/dd");
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class FileSizeFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value.GetType() == typeof(System.UInt64))
            {
                return string.Format("{0:#,0}", (System.UInt64)value);
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
