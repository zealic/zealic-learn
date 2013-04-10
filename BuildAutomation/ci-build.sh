#!/bin/bash
cd `cd $(dirname $0) && pwd`
for name in Travis-CI/*.sh
do
  ./$name
done
