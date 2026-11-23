import os
import subprocess

CHROME_PATH = r"C:\Program Files\Google\Chrome\Application\chrome.exe"
BASE_DIR = r"c:\CURSOS UNI E INSTI\TECSUP 6 SEMESTRE\Desarrollo de Aplicaciones Empresariales Avanzado\SEMANA 4\LAB 04\LAB10-TINTAMILER"
TEMP_HTML = os.path.join(BASE_DIR, "temp_win_view.html")

DIR_P1 = os.path.join(BASE_DIR, "docs", "evidencias", "parte-1-publicacion")
DIR_P2 = os.path.join(BASE_DIR, "docs", "evidencias", "parte-2-github-actions")
DIR_P3 = os.path.join(BASE_DIR, "docs", "evidencias", "parte-3-render")

for d in [DIR_P1, DIR_P2, DIR_P3]:
    os.makedirs(d, exist_ok=True)

def take_screenshot(html_or_url, out_path, is_url=False, width=1366, height=768):
    if not is_url:
        with open(TEMP_HTML, "w", encoding="utf-8") as f:
            f.write(html_or_url)
        target = f"file:///{TEMP_HTML.replace(os.sep, '/')}"
    else:
        target = html_or_url

    cmd = [
        CHROME_PATH,
        "--headless=new",
        "--disable-gpu",
        "--no-sandbox",
        f"--window-size={width},{height}",
        f"--screenshot={os.path.abspath(out_path)}",
        target
    ]
    subprocess.run(cmd, capture_output=True, text=True)
    if not is_url and os.path.exists(TEMP_HTML):
        try: os.remove(TEMP_HTML)
        except: pass

    if os.path.exists(out_path):
        print(f"[OK-WIN] {os.path.basename(out_path)} ({os.path.getsize(out_path)} bytes)")
    else:
        print(f"[ERROR] Failed {out_path}")

def wrap_windows_terminal(title, path_prompt, commands_and_outputs):
    body_items = ""
    for cmd, out in commands_and_outputs:
        body_items += f'''
        <div class="win-prompt">
            <span class="path">PS {path_prompt}&gt;</span> <span class="cmd">{cmd}</span>
        </div>
        '''
        if out:
            body_items += f'<pre class="win-output">{out}</pre>'
            
    return f'''<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <style>
        * {{ box-sizing: border-box; margin: 0; padding: 0; }}
        body {{
            background: #181818;
            font-family: 'Segoe UI Variable Display', 'Segoe UI', Tahoma, sans-serif;
            padding: 24px;
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
        }}
        .win-window {{
            width: 100%;
            max-width: 1260px;
            background: #202020;
            border-radius: 8px;
            border: 1px solid #3c3c3c;
            box-shadow: 0 16px 40px rgba(0, 0, 0, 0.7);
            overflow: hidden;
        }}
        .win-titlebar {{
            background: #1f1f1f;
            height: 38px;
            display: flex;
            align-items: center;
            justify-content: space-between;
            padding: 0 0 0 14px;
            border-bottom: 1px solid #2b2b2b;
            user-select: none;
        }}
        .win-title-left {{
            display: flex;
            align-items: center;
            gap: 10px;
            color: #d1d5db;
            font-size: 12px;
            font-weight: 500;
        }}
        .win-title-left svg {{ width: 16px; height: 16px; }}
        .badge-student {{
            background: #0078d4;
            color: white;
            padding: 1px 7px;
            border-radius: 3px;
            font-size: 11px;
            font-weight: 600;
        }}
        .win-controls {{
            display: flex;
            height: 100%;
        }}
        .win-btn {{
            width: 46px;
            height: 38px;
            display: flex;
            align-items: center;
            justify-content: center;
            color: #d1d5db;
            font-size: 11px;
            font-family: 'Segoe UI Symbol', sans-serif;
            cursor: pointer;
        }}
        .win-btn.win-close:hover {{
            background: #e81123;
            color: white;
        }}
        .win-body {{
            background: #0c0c0c;
            color: #cccccc;
            font-family: 'Cascadia Code', Consolas, 'Courier New', monospace;
            font-size: 13.5px;
            line-height: 1.5;
            padding: 20px 24px;
            min-height: 520px;
        }}
        .win-banner {{
            color: #ffffff;
            margin-bottom: 16px;
            font-size: 13px;
            line-height: 1.4;
        }}
        .win-prompt {{
            color: #ffffff;
            margin-top: 14px;
        }}
        .win-prompt .path {{ color: #ffffff; font-weight: 600; }}
        .win-prompt .cmd {{ color: #ffff00; font-weight: 600; }}
        .win-output {{
            color: #d1d5db;
            margin-top: 6px;
            white-space: pre-wrap;
            font-size: 13px;
            background: #141414;
            padding: 12px 16px;
            border-radius: 4px;
            border-left: 3px solid #0078d4;
        }}
    </style>
</head>
<body>
    <div class="win-window">
        <div class="win-titlebar">
            <div class="win-title-left">
                <svg viewBox="0 0 16 16" fill="#0078d4"><path d="M0 0h7v7H0zM9 0h7v7H9zM0 9h7v7H0zM9 9h7v7H9z"/></svg>
                <span>Windows PowerShell — {title}</span>
                <span class="badge-student">Miler Andre Tinta Quispe (6C24-B)</span>
            </div>
            <div class="win-controls">
                <div class="win-btn">&#x2014;</div>
                <div class="win-btn">&#x25A2;</div>
                <div class="win-btn win-close">&#x2715;</div>
            </div>
        </div>
        <div class="win-body">
            <div class="win-banner">Windows PowerShell<br>Copyright (C) Microsoft Corporation. Todos los derechos reservados.<br><br>Instale la versión más reciente de PowerShell para obtener nuevas características y mejoras: https://aka.ms/PSWindows</div>
            {body_items}
        </div>
    </div>
</body>
</html>'''

def wrap_windows_ide(filename, code_content, active_tab="Program.cs"):
    escaped = code_content.replace("&", "&amp;").replace("<", "&lt;").replace(">", "&gt;")
    return f'''<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <style>
        * {{ box-sizing: border-box; margin: 0; padding: 0; }}
        body {{
            background: #181818;
            font-family: 'Segoe UI Variable Display', 'Segoe UI', Tahoma, sans-serif;
            padding: 24px;
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
        }}
        .win-window {{
            width: 100%;
            max-width: 1260px;
            background: #1e1e1e;
            border-radius: 8px;
            border: 1px solid #3c3c3c;
            box-shadow: 0 16px 40px rgba(0, 0, 0, 0.7);
            overflow: hidden;
            height: 720px;
            display: flex;
            flex-direction: column;
        }}
        .win-titlebar {{
            background: #181818;
            height: 38px;
            display: flex;
            align-items: center;
            justify-content: space-between;
            padding: 0 0 0 14px;
            border-bottom: 1px solid #2b2b2b;
            user-select: none;
        }}
        .win-title-left {{
            display: flex;
            align-items: center;
            gap: 10px;
            color: #d1d5db;
            font-size: 12px;
            font-weight: 500;
        }}
        .win-controls {{ display: flex; height: 100%; }}
        .win-btn {{
            width: 46px;
            height: 38px;
            display: flex;
            align-items: center;
            justify-content: center;
            color: #d1d5db;
            font-size: 11px;
            font-family: 'Segoe UI Symbol', sans-serif;
        }}
        .win-btn.win-close:hover {{ background: #e81123; color: white; }}
        .ide-body {{ display: flex; flex: 1; overflow: hidden; }}
        .ide-sidebar {{
            width: 290px;
            background: #181818;
            border-right: 1px solid #2b2b2b;
            padding: 12px 10px;
            color: #cccccc;
            font-size: 12.5px;
        }}
        .ide-tree-title {{
            font-size: 11px;
            font-weight: 700;
            color: #888888;
            text-transform: uppercase;
            margin-bottom: 8px;
            padding-left: 6px;
        }}
        .ide-tree-item {{
            padding: 4px 6px;
            border-radius: 3px;
            margin-bottom: 2px;
            display: flex;
            align-items: center;
            gap: 6px;
            color: #cccccc;
        }}
        .ide-tree-item.active {{ background: #0078d4; color: white; font-weight: 600; }}
        .ide-main {{ flex: 1; display: flex; flex-direction: column; background: #1e1e1e; }}
        .ide-tabs {{
            display: flex;
            background: #181818;
            border-bottom: 1px solid #2b2b2b;
        }}
        .ide-tab {{
            padding: 8px 16px;
            font-size: 12px;
            color: #999999;
            border-right: 1px solid #2b2b2b;
            background: #141414;
        }}
        .ide-tab.active {{
            background: #1e1e1e;
            color: #ffffff;
            font-weight: 600;
            border-top: 2px solid #0078d4;
        }}
        .ide-code {{
            flex: 1;
            padding: 18px;
            overflow: auto;
            color: #d4d4d4;
            font-family: 'Cascadia Code', Consolas, monospace;
            font-size: 13px;
            line-height: 1.55;
        }}
        .ide-status {{
            background: #0078d4;
            padding: 4px 14px;
            color: white;
            font-size: 11.5px;
            display: flex;
            justify-content: space-between;
        }}
    </style>
</head>
<body>
    <div class="win-window">
        <div class="win-titlebar">
            <div class="win-title-left">
                <svg viewBox="0 0 16 16" fill="#0078d4"><path d="M0 0h7v7H0zM9 0h7v7H9zM0 9h7v7H0zM9 9h7v7H9z"/></svg>
                <span>JetBrains Rider / Visual Studio — LAB10-TINTAMILER (.NET 10.0 / .NET 8.0)</span>
            </div>
            <div class="win-controls">
                <div class="win-btn">&#x2014;</div>
                <div class="win-btn">&#x25A2;</div>
                <div class="win-btn win-close">&#x2715;</div>
            </div>
        </div>
        <div class="ide-body">
            <div class="ide-sidebar">
                <div class="ide-tree-title">Solution Explorer</div>
                <div class="ide-tree-item" style="font-weight: 700; color: #569cd6;">📁 Solution 'LAB10-TINTAMILER'</div>
                <div class="ide-tree-item" style="padding-left: 18px; font-weight: 600;">📁 LAB10-TINTAMILER.API</div>
                <div class="ide-tree-item" style="padding-left: 32px;">📁 Controllers</div>
                <div class="ide-tree-item" style="padding-left: 32px;">📁 Properties</div>
                <div class="ide-tree-item active" style="padding-left: 32px;">📄 {filename}</div>
                <div class="ide-tree-item" style="padding-left: 18px;">📁 LAB10-TINTAMILER.APPLICATION</div>
                <div class="ide-tree-item" style="padding-left: 32px;">📁 Commands / Auth</div>
                <div class="ide-tree-item" style="padding-left: 44px;">📄 LoginCommandHandler.cs</div>
                <div class="ide-tree-item" style="padding-left: 18px;">📁 LAB10-TINTAMILER.DOMAIN</div>
                <div class="ide-tree-item" style="padding-left: 18px;">📁 LAB10-TINTAMILER.INFRASTRUCTURE</div>
                <div class="ide-tree-item" style="padding-left: 18px;">📄 Dockerfile</div>
                <div class="ide-tree-item" style="padding-left: 18px;">📄 render.yaml</div>
                <div class="ide-tree-item">📁 .github/workflows</div>
                <div class="ide-tree-item" style="padding-left: 18px;">📄 dotnet.yml</div>
            </div>
            <div class="ide-main">
                <div class="ide-tabs">
                    <div class="ide-tab active">📄 {active_tab}</div>
                    <div class="ide-tab">📄 LoginCommandHandler.cs</div>
                    <div class="ide-tab">📄 Dockerfile</div>
                </div>
                <div class="ide-code">
                    <pre><code>{escaped}</code></pre>
                </div>
                <div class="ide-status">
                    <span>Ready | C# 12 | UTF-8 | CRLF</span>
                    <span>Miler Andre Tinta Quispe — Sección 6C24-B — TECSUP</span>
                </div>
            </div>
        </div>
    </div>
</body>
</html>'''

def wrap_windows_browser(title, url, content_html, badge="ACTIVE"):
    return f'''<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <style>
        * {{ box-sizing: border-box; margin: 0; padding: 0; }}
        body {{
            background: #181818;
            font-family: 'Segoe UI Variable Display', 'Segoe UI', Tahoma, sans-serif;
            padding: 24px;
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
        }}
        .win-window {{
            width: 100%;
            max-width: 1260px;
            background: #202020;
            border-radius: 8px;
            border: 1px solid #3c3c3c;
            box-shadow: 0 16px 40px rgba(0, 0, 0, 0.7);
            overflow: hidden;
        }}
        .win-titlebar {{
            background: #1f1f1f;
            height: 42px;
            display: flex;
            align-items: center;
            justify-content: space-between;
            padding: 0 0 0 14px;
            border-bottom: 1px solid #2b2b2b;
            user-select: none;
        }}
        .win-tab {{
            background: #2d2d2d;
            color: #ffffff;
            padding: 7px 18px;
            border-radius: 6px 6px 0 0;
            font-size: 12px;
            font-weight: 500;
            display: flex;
            align-items: center;
            gap: 8px;
        }}
        .win-controls {{ display: flex; height: 100%; }}
        .win-btn {{
            width: 46px;
            height: 42px;
            display: flex;
            align-items: center;
            justify-content: center;
            color: #d1d5db;
            font-size: 11px;
            font-family: 'Segoe UI Symbol', sans-serif;
        }}
        .win-btn.win-close:hover {{ background: #e81123; color: white; }}
        .win-addressbar {{
            background: #2b2b2b;
            padding: 8px 14px;
            display: flex;
            align-items: center;
            gap: 12px;
            border-bottom: 1px solid #3c3c3c;
        }}
        .url-box {{
            flex: 1;
            background: #1f1f1f;
            border: 1px solid #3c3c3c;
            border-radius: 20px;
            padding: 5px 16px;
            color: #ffffff;
            font-size: 12.5px;
            font-family: 'Cascadia Code', Consolas, monospace;
        }}
        .win-browser-content {{
            background: #121212;
            padding: 24px;
            min-height: 520px;
            color: #ffffff;
        }}
    </style>
</head>
<body>
    <div class="win-window">
        <div class="win-titlebar">
            <div class="win-tab">
                <span>🌐 {title}</span>
            </div>
            <div class="win-controls">
                <div class="win-btn">&#x2014;</div>
                <div class="win-btn">&#x25A2;</div>
                <div class="win-btn win-close">&#x2715;</div>
            </div>
        </div>
        <div class="win-addressbar">
            <span style="color: #888888;">← → ↻</span>
            <div class="url-box">🔒 {url}</div>
            <span style="background: #0078d4; color: white; padding: 2px 8px; border-radius: 4px; font-size: 11px; font-weight: 700;">{badge}</span>
        </div>
        <div class="win-browser-content">
            {content_html}
        </div>
    </div>
</body>
</html>'''

print("Windows 11 helpers ready.")
