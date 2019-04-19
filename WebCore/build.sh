#!/bin/bash

#docker中执行此脚本进行代码发布构建
solutionName=$1 #解决方案名称
containerName=$2 #容器名称
port=$3 #对外端口
solutionDir=$4 #解决方案路径
csprojDir=$5 #项目路径

#清空之前发布的文件
rm -rf /var/publish/$solutionName/*
echo "正在还原dotnet依赖包"
dotnet restore $solutionDir
echo "正在编译代码到 /var/publish"
dotnet publish $csprojDir -c Release -o /var/publish/$solutionName/
echo "编译成功，准备生成镜像"
docker stop $containerName
docker rm $containerName
docker rmi $containerName
docker build -t $containerName /var/publish/$solutionName/
echo "镜像构建成功"