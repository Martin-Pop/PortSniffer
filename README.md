# 🛠️ Port Sniffer (Windows Forms, C#)

A **port sniffer** application built with **C#** and **Windows Forms**.
It allows you to scan ports on specified IP addresses with configurable ranges, timeouts, and threading.
UI is resizable 👍.

---

## 🧩 Features

- **Flexible IP input:**
  - Single IP address (192.168.1.1)
  - IP range (192.168.1.1 – 192.168.1.50)
  - IP with subnet mask (192.168.1.0/24)

- **Flexible port input:**
  - Single port (80)
  - Port range (20 – 100)
  - Preset checkboxes for:
    - Well-known ports (1–1023)
    - Registered ports (1024–49151)
    - Private/dynamic ports (49152–65535)
    - All ports

- **Custom scan settings:**
  - Timeout (in milliseconds)
  - Maximum threads per IP address

- **Scan control panel:**
  - Start / Stop
  - Pause / Resume
  - Clear - clears outputs and scan configuration

- **Console output:**
  - Displays all events and validation messages
  - Example: IP/port input confirmation, errors, scan started/stopped, etc.
  - Approximate scan duration.
  - Scan progress. 

- **Scan results panel:**
  - Displays all IP addresses where open ports were detected
  - Clicking an address shows the open ports and some other information

- **Config file support:**
  - Allows setting default timeout, thread count, and some UI elements (font, font-size, error color)

---

## ⚠️ Limitations

- The scanner processes IP addresses one by one — scanning waits for one IP to finish before moving to the next. This may cause unnecessary delays for small scans across many IPs.
- Not yet fully optimized for high-speed scanning.
- App doesn't contain export.

---