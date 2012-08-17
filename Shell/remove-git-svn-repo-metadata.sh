#!/bin/sh
git filter-branch --msg-filter ' sed -e '\''/^git-svn-id:/d'\'' | sed -e '\''${/^$/d}'\'' ' --force