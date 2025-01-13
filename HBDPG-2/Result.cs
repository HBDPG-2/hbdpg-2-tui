/*  Copyright (C) 2025 Piotr Kniaz

    This file is part of HBDPG-2.
    Repository: https://github.com/HBDPG-2/hbdpg-2-cli

    Licensed under the MIT License. See LICENSE file in the project root for details.
*/

internal class Result(string? password, double entropy, double elapsedTime, int attempt)
{
    internal string? Password { get => password; }
    internal double Entropy { get => entropy; }
    internal double ElapsedTime { get => elapsedTime; }
    internal int Attempt { get => attempt; }
}