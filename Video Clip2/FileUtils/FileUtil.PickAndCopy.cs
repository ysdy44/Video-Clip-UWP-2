using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Video_Clip2.Clips;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Video_Clip2
{
    public static partial class FileUtil
    {


        /// <summary>
        /// The files picker is displayed so that the user can select a files.
        /// </summary>
        /// <param name="location"> The destination locationId. </param>
        /// <param name="type"> The type file. </param>
        /// <returns> The product files. </returns>
        public async static Task<IReadOnlyList<StorageFile>> PickMultipleImageFilesAsync(PickerLocationId location, ClipType type)
        {
            // Picker
            FileOpenPicker openPicker = null;
            {
                switch (type)
                {
                    case ClipType.Video:
                        openPicker = new FileOpenPicker
                        {
                            ViewMode = PickerViewMode.Thumbnail,
                            SuggestedStartLocation = location,
                            FileTypeFilter =
                            {
                                ".mp4",
                                ".wmv",
                                ".avi",
                                ".webm",
                                ".ts",
                                ".3gp",
                                ".3gpp",
                                ".m4v",
                                ".mov",
                                ".mkv",
                                ".mts",
                                ".m2ts"
                            }
                        };
                        break;
                    case ClipType.Audio:
                        openPicker = new FileOpenPicker
                        {
                            ViewMode = PickerViewMode.Thumbnail,
                            SuggestedStartLocation = location,
                            FileTypeFilter =
                            {
                                ".mp3",
                                ".wmv",
                                ".m4a",
                                ".wav",
                                ".wma"
                            }
                        };
                        break;
                    case ClipType.Image:
                        openPicker = new FileOpenPicker
                        {
                            ViewMode = PickerViewMode.Thumbnail,
                            SuggestedStartLocation = location,
                            FileTypeFilter =
                            {
                                ".jpg",
                                ".jpeg",
                                ".png",
                                ".bmp"
                            }
                        };
                        break;
                    case ClipType.Color:
                        break;
                    case ClipType.Text:
                        break;
                    case ClipType.Subtitle:
                        break;
                    default:
                        break;
                }
            }
            if (openPicker is null) return null;

            // File
            IReadOnlyList<StorageFile> files = await openPicker.PickMultipleFilesAsync();
            return files;
        }

    }
}
