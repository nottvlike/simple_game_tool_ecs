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
*   一个 BaseNotification 目前没实现注册多个 notificationType，暂时没需求；
*   仅仅在 BaseObject 逻辑上使用了 ecs 结构，对于 UI 还是沿用老的代码方式，手游的 UI 算是比较简单的，几乎不会有多个实例的情况存在；
*   AddModule，RemoveModule 和 GetModule 使用到了 c# 的扩展方法功能，这些方法可以放在 BaseObject 里，但是我不希望 BaseObject 太过臃肿；
*   ObjectData 里通过静态成员变量，保存了添加特定 Module 的所有 BaseObject 的 Id，按照 ecs 架构 Module 作为组件不能保存数据， 所以这样虽然有点丑，但也暂时没别的好办法；
*   Data 通过 ValueType 将实例保存在了 ObjectData 里，而 Module 通过 type 保存，BaseObject 通过 id 保存，我之前想用 id 来保存 Data 的，但是 struct 无法继承，通过接口又有装箱拆箱操作；
*   Data 类型没有 Id，因此只能通过 GetData<T>() 获取，这也产生了另一个问题，对于同一个 BaseObject，Data 不能重复；
*   AddData，RemoveData 和 GetData<T> 也使用到了 c# 的扩展方法功能，原因同理；
*   Player 就是玩家自身的一些属性，玩家拥有的角色，物品等其它东西全都算作道具；
*   Common 存放了与具体游戏逻辑无关的代码，Message 将来也会放进去；
*   struct 都实现了 IEquatable 接口，主要是预防直接比较结构体而产生的装箱拆箱操作；
*   为了可移植性，将 unity 的代码尽可能的放一个地方，尽可能少的减少 unity 的 api 对于整体框架的影响；
*   我还是喜欢按照目录把同一个系统的东西放一起。

以后想到了再补充...

## 注意
我这里的开发版本是 unity 5.4.1，ResourceManager 里面之前用到了 litjson，后来又改成了 unity5 的 json 接口，所以低版本 unity 想要适配的话，需要自己改动下 ResourceManager 里的代码。

之前的代码都是在 oschina 提交的，所以 github 没有提交日志。