cmake_minimum_required(VERSION 2.6)

PROJECT(GdiPlus)
LINK_LIBRARIES(gdi32.lib gdiplus.lib)

IF(MINGW)
  MESSAGE(STATUS "HELLO GCC")
  SET(CMAKE_CXX_FLAGS "-static-libgcc -static-libstdc++")
ENDIF(MINGW)

ADD_EXECUTABLE(CreateDIBSection CreateDIBSection.cpp)
