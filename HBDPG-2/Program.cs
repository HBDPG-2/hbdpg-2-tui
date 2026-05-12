/*  Copyright (C) 2025-2026 Piotr Kniaz

    This file is part of HBDPG-2.
    Repository: https://github.com/HBDPG-2/hbdpg-2-tui

    Licensed under the MIT License. See LICENSE file in the project root for details.
*/

using Terminal.Gui.App;
using HBDPG2.UI;

using var app = Application.Create().Init();
var top = new MainWindow(app);
app.Run(top);
top.Dispose();
