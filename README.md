# MindNote

[![](https://img.shields.io/github/stars/StardustDL/MindNote.svg?style=social&label=Stars)](https://github.com/StardustDL/MindNote) [![](https://img.shields.io/github/forks/StardustDL/MindNote.svg?style=social&label=Fork)](https://github.com/StardustDL/MindNote) ![](http://progressed.io/bar/10?title=developing) [![](https://img.shields.io/github/license/StardustDL/MindNote.svg)](https://github.com/StardustDL/MindNote/blob/master/LICENSE)

<p style="text-align:center; font-size:21px">
Empower your notes.
</p>

Project Status:

|||
|-|-|
|Repository|[![issue](https://img.shields.io/github/issues/StardustDL/MindNote.svg)](https://github.com/StardustDL/MindNote/issues/) [![pull requests](https://img.shields.io/github/issues-pr/StardustDL/MindNote.svg)](https://github.com/StardustDL/MindNote/pulls/)|
|Dependencies|[![dependencies](https://img.shields.io/librariesio/github/StardustDL/MindNote.svg)](https://libraries.io/github/StardustDL/MindNote)|
|Build|[![master](https://img.shields.io/travis/StardustDL/MindNote/master.svg?label=master)](https://travis-ci.org/StardustDL/MindNote) [![dev](https://img.shields.io/travis/StardustDL/MindNote/dev.svg?label=dev)](https://travis-ci.org/StardustDL/MindNote)|
|Coverage|[![master](https://img.shields.io/codecov/c/github/StardustDL/MindNote/master.svg?label=master)](https://codecov.io/gh/StardustDL/MindNote) [![dev](https://img.shields.io/codecov/c/github/StardustDL/MindNote/dev.svg?label=dev)](https://codecov.io/gh/StardustDL/MindNote)|

# Usage

- Install Docker and Docker-Compose
- Use the commands below to create containers:

```sh
curl https://raw.githubusercontent.com/StardustDL/MindNote/dev/docker/docker-compose.yml > docker-compose.yml
docker-compose up
```

It will create a MySQL container, an API server container, and a host server container.

The host server listens to the port `8085`.
