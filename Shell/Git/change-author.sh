#!/bin/sh
: """
if ! options=$(getopt --long name:,new-name:,new-mail: -- "$@")
then
  echo 'Invalid options.'
  exit 1
fi

set -- $options

while true; do
  case "$1" in
    --name ) OLD_NAME="$2"; shift 2 ;;
    --new-name ) NEW_NAME="$2"; shift 2 ;;
    --new-mail ) NEW_MAIL="$2"; shift 2 ;;
    * ) break ;;
  esac
done
"""
OLD_NAME="$1"
shift
NEW_NAME="$1"
shift
NEW_MAIL="$1"

git filter-branch --commit-filter '
  if [ "$GIT_COMMITTER_NAME" = "'"$OLD_NAME"'" ];
  then
    GIT_COMMITTER_NAME="'"$NEW_NAME"'";
    GIT_AUTHOR_NAME="'"$NEW_NAME"'";
    GIT_COMMITTER_EMAIL="'"$NEW_MAIL"'";
    GIT_AUTHOR_EMAIL="'"$NEW_MAIL"'";
    git commit-tree "$@";
  else
    git commit-tree "$@";
  fi' HEAD
