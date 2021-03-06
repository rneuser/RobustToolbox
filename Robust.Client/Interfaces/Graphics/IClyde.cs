﻿using System;
using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Robust.Client.Graphics;
using Robust.Shared.Maths;

namespace Robust.Client.Interfaces.Graphics
{
    public interface IClyde
    {
        Vector2i ScreenSize { get; }
        void SetWindowTitle(string title);
        event Action<WindowResizedEventArgs> OnWindowResized;

        Texture LoadTextureFromPNGStream(Stream stream, string? name = null,
            TextureLoadParameters? loadParams = null);

        Texture LoadTextureFromImage<T>(Image<T> image, string? name = null,
            TextureLoadParameters? loadParams = null) where T : unmanaged, IPixel<T>;

        IRenderTarget CreateRenderTarget(Vector2i size, RenderTargetFormatParameters format,
            TextureSampleParameters? sampleParameters = null, string? name = null);

        void CalcWorldProjectionMatrix(out Matrix3 projMatrix);

        // Cursor API.
        /// <summary>
        ///     Gets a cursor object representing standard cursors that match the OS styling.
        /// </summary>
        /// <remarks>
        ///     Cursor objects returned from this method are cached and you cannot not dispose them.
        /// </remarks>
        ICursor GetStandardCursor(StandardCursorShape shape);

        /// <summary>
        ///     Create a custom cursor object from an image.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="hotSpot"></param>
        /// <returns></returns>
        ICursor CreateCursor(Image<Rgba32> image, Vector2i hotSpot);

        /// <summary>
        ///     Sets the active cursor for the primary window.
        /// </summary>
        /// <param name="cursor">The cursor to set to, or <see langword="null"/> to reset to the default cursor.</param>
        /// <exception cref="ObjectDisposedException">Thrown if the cursor object passed has been disposed.</exception>
        void SetCursor(ICursor? cursor);

        /// <summary>
        ///     Make a screenshot of the game, next render frame.
        /// </summary>
        /// <param name="type">What kind of screenshot to take</param>
        /// <param name="callback">The callback to run when the screenshot has been made.</param>
        void Screenshot(ScreenshotType type, Action<Image<Rgb24>> callback);

        Task<Image<Rgb24>> ScreenshotAsync(ScreenshotType type)
        {
            var tcs = new TaskCompletionSource<Image<Rgb24>>();

            Screenshot(type, image => tcs.SetResult(image));

            return tcs.Task;
        }
    }

    // TODO: Maybe implement IDisposable for render targets. I got lazy and didn't.
}
