/*---
- title: 'AOP Main'
- category: ['C', 'VC']
- references: {
    'Running Code Before and After Main': 'http://www.codeguru.com/cpp/misc/misc/applicationcontrol/article.php/c6945/Running-Code-Before-and-After-Main.htm'
  }
---*/
#include <windows.h>

#pragma comment(lib, "user32.lib")

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, PSTR szCmdLine, int iCmdShow)
{
   MessageBox(NULL, TEXT("Hello, Main Program!"), TEXT("Hello"), 0);
   return 0;
}
int PreMain1(void)
{
   MessageBox(NULL, TEXT("First Pre-Main Function"), TEXT("PreMain1"), 0);
   return 0;
}
int PreMain2(void)
{
   MessageBox(NULL, TEXT("Second Pre-Main Function"),TEXT("PreMain2"), 0);
   return 0;
}
int PostMain1(void)
{
   MessageBox(NULL, TEXT("First Post-Main Function"), TEXT("PostMain1"), 0);
   return 0;
}
int PostMain2(void)
{
   MessageBox(NULL, TEXT("Second Post-Main function"),
                    TEXT("PostMain2"), 0);
   return 0;
}
typedef int cb(void);

#pragma data_seg(".CRT$XIU")
static cb *autostart[] = { PreMain1, PreMain2 };

#pragma data_seg(".CRT$XPU")
static cb *autoexit[] = { PostMain1, PostMain2 };

#pragma data_seg()    /* reset data-segment */
