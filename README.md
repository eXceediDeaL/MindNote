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

1. Install Docker and Docker-Compose.
2. Clone this repository.
3. Enter the deploy directory is `docker/deploy`. 
4. Specify the configuration in `config` directory.
5. Use these commands to start:

```sh
docker-compose up -d
```

It will create a MySQL container, an API server container, an identity server container, a host server container and a nginx server container for reverse proxy.

The default hostnames are:

- `id.mindnote.com`
- `api.mindnote.com`
- `mindnote.com`
