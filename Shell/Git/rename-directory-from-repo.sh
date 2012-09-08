#!/bin/sh
git filter-branch -f --prune-empty --index-filter \
  'git ls-files -s | sed "s-OMG/-OhMyGod/-" | \
  GIT_INDEX_FILE=$GIT_INDEX_FILE.new git update-index --index-info && \
  mv "$GIT_INDEX_FILE.new" "$GIT_INDEX_FILE"' HEAD
