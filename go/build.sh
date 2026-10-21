#!/usr/bin/env bash
# 统一构建入口：编译缓存与临时产物全部落在仓库 temp/ 下，避免杀毒软件反复扫描仓库目录。
set -e
cd "$(dirname "$0")"
export GOROOT="C://Users//ASUS//.workbuddy//binaries//go//go"
export GOCACHE="$(cd .. && pwd)/temp/go-cache"
export GOTMPDIR="$(cd .. && pwd)/temp/go-tmp"
export PATH="/c/Users/ASUS/.workbuddy/binaries/go/go/bin:$PATH"
exec go "$@"
