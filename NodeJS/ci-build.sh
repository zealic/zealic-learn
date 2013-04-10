#!/bin/bash
for name in */
do
  cd name
  npm test
done
