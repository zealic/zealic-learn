/*---
- title: 'Function Attributes : constructor and destructor'
- category: ['C++']
- references: {
    'Function Attributes': 'http://gcc.gnu.org/onlinedocs/gcc-4.1.1/gcc/Function-Attributes.html'
  }
---*/
#include <iostream>

struct Foo {
  Foo() { std::cout << "Foo created!" << std::endl; }
};
static Foo foo;

int main() {
  std::cout << "Hello Foo!";
  return 0;
}
