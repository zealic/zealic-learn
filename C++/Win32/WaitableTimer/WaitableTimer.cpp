#include <windows.h>
#include <iostream>

#pragma comment(lib, "winmm.lib")

using namespace std;

class Stopwatch {
  public:
    Stopwatch() { 
      QueryPerformanceFrequency(&m_Freq);
      Start();
    }
    
    void Start() {
      QueryPerformanceCounter(&m_Start);
    }
    
    __int64 ElapsedTime() const {
      LARGE_INTEGER now;
      QueryPerformanceCounter(&now);
      return (((now.QuadPart - m_Start.QuadPart) * 1000) / m_Freq.QuadPart);
    }
    
  private:
    LARGE_INTEGER m_Freq;
    LARGE_INTEGER m_Start;
};

void DoWait(HANDLE hTimer, __int64 time, bool noOutput = false) {
    LARGE_INTEGER durTime;
    durTime.QuadPart = -time * 10000;
    if(!noOutput) printf("Waiting for %d...\n", time);
    
    // Set a timer to wait
    if (!SetWaitableTimer(hTimer, &durTime, 0, NULL, NULL, 0)) {
        printf("SetWaitableTimer failed (%d)\n", GetLastError());
        throw 1;
    }
    
    // Wait for the timer.
    if (WaitForSingleObject(hTimer, INFINITE) != WAIT_OBJECT_0)
      printf("WaitForSingleObject failed (%d)\n", GetLastError());
    else
      if(!noOutput) printf("Timer was signaled.\n");
}

int main()
{
    Stopwatch stopwatch;
    HANDLE hTimer = NULL;

    // Create an unnamed waitable timer.
    hTimer = CreateWaitableTimer(NULL, TRUE, NULL);
    if (NULL == hTimer) {
        printf("CreateWaitableTimer failed (%d)\n", GetLastError());
        return 1;
    }
    
    // With Sleep
    stopwatch.Start();
    DoWait(hTimer, 1000);
    printf("Wait 1000ms, Elapsed time : %d\n", stopwatch.ElapsedTime());

    // With Sleep
    stopwatch.Start();
    DoWait(hTimer, 1234);
    Sleep(100);
    printf("Wait 1234ms with Sleep(100), Elapsed time : %d\n", stopwatch.ElapsedTime());

    return 0;
}
