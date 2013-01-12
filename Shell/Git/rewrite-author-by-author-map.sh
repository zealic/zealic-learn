#!/bin/bash
author_map_file=$(readlink -f "$1")
if [ ! -f "$author_map_file" ]; then
  echo "authors-map file $author_map_file not exist"
  exit 1
fi
mkdir /tmp/git-rewrite-user
cat "$author_map_file" > /tmp/git-rewrite-user/authors-map.txt
cat > /tmp/git-rewrite-user/core.sh << \EOF
line=$(grep -F "$GIT_COMMITTER_NAME" /tmp/git-rewrite-user/authors-map.txt)
rawName=$(echo "$line" | sed -r 's/^\s*(.*)\s*=.*$/\1/' | sed 's/ *$//g')
cname=$(echo "$line" | sed -r 's/^.*=\s*(.*)\s*<.*$/\1/' | sed 's/ *$//g')
email=$(echo "$line" | sed -r 's/^.*=.*<\s*(.*)\s*>\s*$/\1/' | sed 's/ *$//g')

if [[ "$rawName" == "$GIT_COMMITTER_NAME" ]]; then
  GIT_COMMITTER_NAME="$cname" &&
  GIT_COMMITTER_EMAIL="$email" &&
  GIT_AUTHOR_NAME="$GIT_COMMITTER_NAME" &&
  GIT_AUTHOR_EMAIL="$GIT_COMMITTER_EMAIL" &&
  export GIT_AUTHOR_NAME GIT_AUTHOR_EMAIL &&
  export GIT_COMMITTER_NAME GIT_COMMITTER_EMAIL
fi
EOF

git filter-branch -f --env-filter '
source /tmp/git-rewrite-user/core.sh
' HEAD

rm -r /tmp/git-rewrite-user
