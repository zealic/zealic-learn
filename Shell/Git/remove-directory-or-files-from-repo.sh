git filter-branch -f --prune-empty --index-filter \
  'git rm -r -f --cached --ignore-unmatch my_secret_dir/ my_secret_file your_secret_file-*' HEAD
