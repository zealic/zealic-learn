/*---
- title: 'Function Attributes : constructor and destructor'
- category: ['C', 'GCC']
- references: {
    'Function Attributes': 'http://gcc.gnu.org/onlinedocs/gcc-4.1.1/gcc/Function-Attributes.html'
  }
---*/
#include <stdio.h>

__attribute__((constructor)) void before_main()
{
   printf("before main\n");
}

__attribute__((destructor)) void after_main()
{
   printf("after main\n");
}

int main()
{
  printf("in main\n");
  return 0;
}
