: << EOF
@bash  %~fs0
@goto :eof
EOF
#!/bin/bash
# Bash on batch file
# You can execute shell script or batch script on one file!
msg="Hello World!"
echo $msg
