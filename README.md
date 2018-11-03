# SimpleGameTool
unity 客户端游戏框架

## 简介
之前写过一款同样叫 SimpleGameTool 的框架（后来我给它改名成 SimpleGameToolLua 了），用 C# 构建了底层，顶层逻辑完全是用 lua 实现的，后来发现了几个问题：

*   lua 没有很好的数据保护的功能，我希望公开一些 api 给用户扩展，同时又能保护重要的数据这一点很难实现；
*   逻辑越复杂，之前的架构就越混乱，感觉逻辑越来越难实现了。

因此我在搭建这个框架时暂时完全没考虑 lua ,架构方面的优化则主要源自于之前恰好看过的一点 ecs 文档。

## 想法
写一些自己使用某些技术搭建框架的原因（或者说添加某些代码的原因）：

*   Notification 模块，作为连接各个模块的公路，降低各个模块之前的耦合；
*   Factory 和一些接口将各个系统完全隔绝起来了，记得 ogre 好像用过这种，但是我倒没热插拔这种需求；
*   各个系统内部通信，能用委托就用委托就行了；
*   添加 NotificationMode 区分 Notification 是 Object 还是 ValueType， 是为了通信的装箱，拆箱操作；
*   ObjectData 里通过静态成员变量，保存了添加特定 Module 的所有 BaseObject 的 Id，按照 ecs 架构 Module 作为组件不能保存数据， 所以这样虽然有点丑，但也暂时没别的好办法；
*   Data改为 Class了，用struct没法继承的话不好写
*   Data 类型没有 Id，因此只能通过 GetData<T>() 获取，这也产生了另一个问题，对于同一个 BaseObject，Data 不能重复；
*   Common 存放了与具体游戏逻辑无关的代码；
*   为了可移植性，将 unity 的代码尽可能的放一个地方，尽可能少的减少 unity 的 api 对于整体框架的影响；

## 注意
我这里的开发版本是 unity 2018.2.7，ResourceManager 里面之前用到了 litjson，后来又改成了 unity5 的 json 接口，所以低版本 unity 想要适配的话，需要自己改动下 ResourceManager 里的代码。

之前的代码都是在 oschina 提交的，所以 github 没有提交日志。
