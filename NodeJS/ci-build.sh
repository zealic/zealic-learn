#!/bin/bash
cd `cd $(dirname $0) && pwd`
for name in */
do
  cd $name
  npm test
done
