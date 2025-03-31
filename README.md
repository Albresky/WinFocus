# WinFocus
Windows Focus Wallpaper | Windows 聚焦壁纸


# Preview

https://github.com/user-attachments/assets/00af664e-bff4-4bcc-82d0-9bfdb348f6ea


# Installation

 - 下载 v2 版本 [安装包与根证书](https://github.com/Albresky/WinFocus/releases)
 - 双击证书，安装到 `受信任的根证书颁发机构` 
 - 双击 `WinFocus.msix`，安装主程序


# Features
 - 支持Windows锁屏出现的历史壁纸的预览与缓存
   - （系统会定期清理历史聚焦壁纸）
 - 多样的超清 UHD 必应壁纸
 - 自定义桌面动态壁纸
 - 自定义Focus锁屏壁纸
 - ...


# Implementations
- [x] 系统支持
  - [x] 支持Windows 11 （<=24H1）
  - [x] 支持Windows 10 （1803+）
- [x] 全新WinUI 3框架
- [x] 支持高分屏
  - [x] 2K
  - [x] 2.5K+
  - [x] 4K
- [ ] 必应超清壁纸
  - [x] 2160P+（4K+）
  - [x] 1200P
  - [x] 1080P
  - [x] 720P
  - [x] 壁纸简介
- [x] 核心采用Win32 API实现
  - [x] 动态壁纸
  - [x] 桌面壁纸
- [x] More


# Known Issues

 - 较新版本的 Windows 11 (24H2) 的动态壁纸设置可能无法正常工作。
---

# TODO
- [ ] Focus聚焦壁纸
  - [x] 获取及预览
  - [x] 本地缓存
  - [ ] 设置聚焦壁纸
- [x] 动态壁纸
  - [x] 视频预览
  - [x] 本地导入
  - [x] 设置动态桌面壁纸[**支持音轨**]
- [ ] 必应壁纸
  - [x] 在线获取及缓存
  - [ ] 预览与设置
- [x] 设置桌面静态壁纸
- [ ] 在线更新
- [ ] 使用向导
