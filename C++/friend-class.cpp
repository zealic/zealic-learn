#include <iostream>

class Depot;
class Guarder;

class Depot {
  private:
    int m_ItemCount;

  public:
    friend class Guarder;
    
    Depot() {
      m_ItemCount = 120;
    }
    
    int count() {
      return m_ItemCount;
    }
};

class Guarder {
  public:
    void filch(Depot &depot) {
      depot.m_ItemCount = depot.m_ItemCount - (depot.m_ItemCount - 1);
    }
};

int main(int argc, char **argv) {
  using namespace std;
  
  Depot depot;
  cout << "M items count is " << depot.count() << endl;
  Guarder jeff;
  
  jeff.filch(depot);
  cout << "M items count is " << depot.count() << endl;
  if(depot.count() < 100) {
    cout << "What the fuck! who stealing depot's items? " << endl;
  }
  
  return 0;
}
