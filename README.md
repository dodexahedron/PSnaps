# PSnaps
PowerShell module for managing Snap packages on Linux.

## Installation
Install-Module PSnaps -AllowPrerelease

## Uninstallation
Uninstall-Module PSnaps

## Example Usage:

```powershell
Get-SnapPackage -All
```
Output:
```text
Name                                             Version Revision Tracking                   Publisher          Publisher Verified
----                                             ------- -------- --------                   ---------          ------------------
bare                                                 1.0        5 latest/stable              Canonical                 True
core20                                          20250822     2682 latest/stable              Canonical                 True
core22                                          20250923     2139 latest/stable              Canonical                 True
core24                                          20251001     1225 latest/stable              Canonical                 True
desktop-security-center                    0+git.d93b42d       99 1/stable/ubuntu-25.10      Canonical                 True
firefox                                        144.0.2-1     7177 latest/stable/ubuntu-25.10 Mozilla                   True
firefox                                          145.0-2     7298 latest/stable/ubuntu-25.10 Mozilla                   True
firmware-updater                           0+git.0052f6b      210 1/stable/ubuntu-25.10      Canonical                 True
gnome-42-2204             0+git.837775c-sdk0+git.7b07595      226 latest/stable              Canonical                 True
gtk-common-themes                        0.1-81-g442e511     1535 latest/stable              Canonical                 True
lz4                                                1.9.4        4 latest/stable              Edward Hope-Morley       False
mesa-2404                                 25.0.7-snap211     1165 latest/stable/ubuntu-25.10 Canonical                 True
powershell-preview                       7.6.0-preview.5      179 latest/stable              Canonical                 True
prompting-client                           0+git.d542a5d      104 1/stable/ubuntu-25.10      Canonical                 True
snap-store                                0+git.90575829     1270 2/stable/ubuntu-25.10      Canonical                 True
snapd                                               2.72    25577 latest/stable              Canonical                 True
snapd-desktop-integration                            0.9      315 latest/stable/ubuntu-25.10 Canonical                 True
thunderbird                                 140.4.0esr-1      846 latest/stable/ubuntu-25.10 Canonical                 True
```
