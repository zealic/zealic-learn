/*---
- title: 'Win32 API : CreateDIBSection'
- category: ['C++', 'Windows', 'GdiPlus']
- references: {
    'MSDN - CreateDIBSection': 'http://msdn.microsoft.com/en-us/library/windows/desktop/dd183494(v=vs.85).aspx'
  }
---*/
#include <iostream>
#include <windows.h>
#include <gdiplus.h>

using namespace std;
using namespace Gdiplus;

int main() {
  // Initialize GDI+.
  GdiplusStartupInput gdiplusStartupInput;
  ULONG_PTR gdiplusToken; 
  GdiplusStartup(&gdiplusToken, &gdiplusStartupInput, NULL);
  
  HDC hdc = CreateCompatibleDC(NULL);
  cout << "BASE HDC = " << hdc << endl;;
  
  BITMAPINFO *bi32;
  long size = sizeof(BITMAPINFO) + sizeof(RGBQUAD) * 256;
  bi32 = (BITMAPINFO*)malloc(size);
  memset(bi32, 0 , size);
  bi32->bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
  bi32->bmiHeader.biWidth = 200;
  bi32->bmiHeader.biHeight = 200;
  bi32->bmiHeader.biPlanes = 1;
  bi32->bmiHeader.biBitCount = 32;
  bi32->bmiHeader.biCompression = BI_RGB;
  
  BITMAPINFO stBmi = {0};
  stBmi.bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
  stBmi.bmiHeader.biWidth = 200;
  stBmi.bmiHeader.biHeight = 200;
  stBmi.bmiHeader.biPlanes = 1;
  stBmi.bmiHeader.biBitCount = 16;
  stBmi.bmiHeader.biCompression = BI_RGB;
  
  void *pvBits;
  HBITMAP hbmp1 = CreateDIBSection(NULL, bi32, DIB_RGB_COLORS, &pvBits, NULL, 0);
  cout << "BASE HBMP1 = " << hbmp1 << ", " << pvBits << endl;
  HBITMAP hbmp2 = CreateDIBSection(NULL, &stBmi, DIB_RGB_COLORS, &pvBits, NULL, 0);
  cout << "BASE HBMP2 = " << hbmp2 << ", " << pvBits << endl;
  return 0;
}