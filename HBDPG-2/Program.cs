/*  Copyright (C) 2025 Piotr Kniaz

    This file is part of HBDPG-2.
    Repository: https://github.com/HBDPG-2/hbdpg-2-tui

    Licensed under the MIT License. See LICENSE file in the project root for details.
*/

using Terminal.Gui;
using HBDPG2.UI;

Application.Init();

var mainWindow = new MainWindow();

Application.Run(mainWindow);
Application.Shutdown();