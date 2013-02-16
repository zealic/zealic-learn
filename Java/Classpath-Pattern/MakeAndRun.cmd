: """
@bash  %~fs0
@goto :eof
"""
#!/bin/bash
mkdir classes 2> NUL
#javac -d classes Echo.java
javac EntryPoint.java
rm Echo.class
java -cp ".;classes" EntryPoint
