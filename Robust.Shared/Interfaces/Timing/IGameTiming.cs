﻿using System;
using JetBrains.Annotations;
using Robust.Shared.Timing;

namespace Robust.Shared.Interfaces.Timing
{
    /// <summary>
    ///     This holds main loop timing information and helper functions.
    /// </summary>
    public interface IGameTiming
    {
        /// <summary>
        /// Is program execution inside of the simulation, or rendering?
        /// </summary>
        bool InSimulation { get; set; }

        /// <summary>
        ///     Is the simulation currently paused?
        /// </summary>
        bool Paused { get; set; }

        /// <summary>
        ///     The current synchronized uptime of the simulation. Use this for in-game timing. This can be rewound for
        ///     prediction, and is affected by Paused and TimeScale.
        /// </summary>
        TimeSpan CurTime { get; }

        /// <summary>
        ///     The current real uptime of the simulation. Use this for UI and out of game timing.
        /// </summary>
        TimeSpan RealTime { get; }

        /// <summary>
        ///     The simulated time it took to render the last frame.
        /// </summary>
        TimeSpan FrameTime { get; }

        /// <summary>
        ///     The real time it took to render the last frame.
        /// </summary>
        TimeSpan RealFrameTime { get; }

        /// <summary>
        ///     Average real frame time over the last 50 frames.
        /// </summary>
        TimeSpan RealFrameTimeAvg { get; }

        /// <summary>
        ///     Standard Deviation of the frame time over the last 50 frames.
        /// </summary>
        TimeSpan RealFrameTimeStdDev { get; }

        /// <summary>
        ///     Current graphics frame since init OpenGL which is taken as frame 1. Useful to set a conditional breakpoint on specific frames, and
        ///     synchronize with OGL debugging tools that capture frames. Depending on the tools used, this frame
        ///     number will vary between 1 frame more or less due to how that tool is counting frames,
        ///     i.e. starting from 0 or 1, having a separate counter, etc. Available in timing debug panel.
        /// </summary>
        uint CurFrame { get; set; }

        /// <summary>
        ///     Average real FPS over the last 50 frames.
        /// </summary>
        double FramesPerSecondAvg { get; }

        /// <summary>
        ///     The current simulation tick being processed.
        /// </summary>
        GameTick CurTick { get; set; }

        /// <summary>
        ///     The target ticks/second of the simulation.
        /// </summary>
        byte TickRate { get; set; }

        /// <summary>
        ///     The length of a tick at the current TickRate. 1/TickRate.
        /// </summary>
        TimeSpan TickPeriod { get; }

        /// <summary>
        /// The remaining time left over after the last tick was ran.
        /// </summary>
        TimeSpan TickRemainder { get; set; }

        /// <summary>
        ///     If the client clock is a little behind or ahead of the server, you can
        ///     use the to adjust the timing of the clock speed. The default value is 0,
        ///     and you can run the clock from -1 (almost stopped) to 1 (almost no delay).
        ///     This has no effect on in-simulation timing, and only changes the speed at which
        ///     the simulation progresses in relation to Real time. Don't mess with this unless
        ///     you know what you are doing. DO NOT TOUCH THIS ON SERVER.
        /// </summary>
        float TickTimingAdjustment { get; set; }

        /// <summary>
        ///     Ends the 'lap' of the timer, updating frame time info.
        /// </summary>
        void StartFrame();

        /// <summary>
        ///     Resets the real uptime of the server.
        /// </summary>
        void ResetRealTime();

        bool IsFirstTimePredicted { get; }

        void StartPastPrediction();
        void EndPastPrediction();

        [MustUseReturnValue]
        PredictionGuard StartPastPredictionArea()
        {
            StartPastPrediction();

            return new PredictionGuard(this);
        }

        string TickStamp => $"{CurTick}, predFirst: {IsFirstTimePredicted}";
    }
}
