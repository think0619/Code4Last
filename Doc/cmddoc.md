**指令说明**

`cmd${DestType}${Dest}_{CMDContent}`

| 内容 | 解释 | 
| :- | :- | 
| cmd | 标识命令，固定开头 |
| {DestType} | 目标类型，目前现有"PC"和"Model" |
| {Dest} | 目标位置，比如模型IP或者计算机标识符 |
| {CMDContent} | 指令内容|
| $ | 分隔符 |

示例
***
- 模型控制指令
  * 固定IP单播发送
  * `cmd$Model$M1_open01`
  * 向m1模型发送指令 "open01"
- PC端软件指令示例
  * 广播发送 255.255.255.255
  * `cmd$PC$V4DES_Video_PV01`
  * 向标识符为V4DES发送指令 "Video_PV01"。"V4DES"为计算机唯一标识符，5位数字+字幕组合