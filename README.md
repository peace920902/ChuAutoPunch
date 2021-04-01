# 自動簽到

## 免責聲明 

**此程式為本人依興趣開發，本人不負任何未成功簽到之責任，所有使用程式進行非法行為者，本人不負任何法律責任**

## 重點


* **請勿用於代簽**
* **此功能一樣須在教室才能成功(因為要連網路)**
* **電腦必須是開機狀態(筆電不能關著)**
* **此功能僅限windows 系統**
* **每天第一次執行會比較慢一點，之後會做修正**
* **本功能需安裝 .net 5 runtime 原則上有裝 visual studio 就有
沒有的話請到以下網址安裝(https://dotnet.microsoft.com/download/dotnet/5.0)**
* **有進階功能(不開啟chrome，手動按下簽到按鈕等等)，因為我懶得寫文件，如果需要可以來詢問我**


原始碼放在我的github(https://github.com/peace920902/ChuAutoPunch) **用了請給星星 不然上廁所都沒有衛生紙**
![](https://i.imgur.com/5QZEkQy.png)

> 碎碎念: 寫文件的時間都要比寫code多了Orz

## 步驟1
到 https://drive.google.com/file/d/1wuEfibJM5RLWItV1Qsa-TZV6IBy-6kCJ/view?usp=sharing 下載程式

下載完解壓縮會有一個 .net 5.0 的資料夾
![](https://i.imgur.com/2jhVwGS.png)
打開資料夾

## 步驟2

點開appsetting.json 文件
有些人沒開副檔名

[附錄 打開副檔名](https://hackmd.io/@lazcat/SJ3l3pZBd#%E9%99%84%E9%8C%84-%E6%89%93%E9%96%8B%E5%89%AF%E6%AA%94%E5%90%8D)

![](https://i.imgur.com/j0bEhcF.png)



內容看起來像
![](https://i.imgur.com/dpbDCD3.png)

接著將(見下圖)
**綠色框改成你的網路帳號(XXXX@ondemand)**
**黃色框改成你的網路密碼**
**紅色框改成你的學生平台帳號**
**紫色框改成你的學生平台密碼**
(都是紙條上的，請保留"")

如圖:
![](https://i.imgur.com/nG3GOAy.png)

## 步驟3
到搜索找到工作排程器

![](https://i.imgur.com/jmgIomu.png)

選擇建立工作
![](https://i.imgur.com/6RY14LC.png)

名稱請隨意輸入
![](https://i.imgur.com/bhbVT5z.png)

接著點擊觸發程序
![](https://i.imgur.com/5vFY3cn.png)

選擇新增
![](https://i.imgur.com/uTII0qg.png)


1. 勾選每周
2. 選擇**上午** 10:15:25 (秒數跟分鐘可以自訂，秒數盡量不要00秒，不然萬一還沒開起簽到功能就尷尬了，個人建議每組每個人的分鐘秒數要錯開，不然wireless 的網路一次多人連程式會爆炸)
3. 勾選周1-4(上課時間)
4. 選擇確定

![](https://i.imgur.com/KUvNOil.png)


接著重複三次新增觸發程序
**但是將第2點的時間分別改為 下午12:15 下午01:30 下午06:00**

做完會像這樣  
![](https://i.imgur.com/Hnqf0yI.png)

### 接著點擊動作
一樣選擇新增  
![](https://i.imgur.com/h74a5Q2.png)

動作選擇啟動程式  
![](https://i.imgur.com/z7fRp6L.png)

程式碼或指令碼請按瀏覽
找後找到前面下載的 .net 5.0資料夾中的 **AutoAttend.exe**
![](https://i.imgur.com/Sig6EWH.png)

然後點擊確定->確定 就大功告成了

## 測試程式是否正常運行(見下面[demo](https://hackmd.io/@lazcat/SJ3l3pZBd#demo))

**此測試一樣要在教室做**
回到步驟三
新增一個離現在比較近的時間
例如我現在時間是晚上8:52
我將時間設定為8:55
然後存檔
等到8:55 如果程式有啟動就有成功了
然後再把剛新增的右鍵刪除
![](https://i.imgur.com/Avdp501.png)

預設會有10秒的確認成功簽到的時間(10秒到chrome 會自動關閉)

## 因為程式執行完wifi 會切到 wireless 去，請大家手動切回教室網路，不然我怕有人連不到

## demo
原先會開啟兩個網頁
一個用來連接wifi
因為我demo的時候是不在教室的時間
所以連不到wifi

![](https://imgur.com/3HbBcVe.gif)

## 附錄 打開副檔名
開啟檔案總管
選擇檢視

![](https://i.imgur.com/cubd60F.png)

將隱藏的項目跟副檔名打勾

![](https://i.imgur.com/zXz57QA.png)

