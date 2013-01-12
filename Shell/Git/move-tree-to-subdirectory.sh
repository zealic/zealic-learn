#!/bin/sh
git filter-branch --index-filter ' 
  rm -f "$GIT_INDEX_FILE" 
  git read-tree --prefix=newsubdirectory/ "$GIT_COMMIT" 
' HEAD
