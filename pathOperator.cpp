#include "pathOperator.h"
#include "commonHeaders.h"

bool getUserName(QString& username)
{
    const int MAX_LENGTH=256;
    wchar_t buffer[MAX_LENGTH];
    DWORD len=MAX_LENGTH;
    if(GetUserName(buffer, &len))
    {
        username=QString::fromWCharArray(buffer,-1);
        qDebug()<<"Username:"<<username;
        return true;
    }
    return false;
}

bool getWinPath(QString& str)
{
    const int MAX_LENGTH=256;
    wchar_t buffer[MAX_LENGTH];
    if(GetWindowsDirectory(buffer, MAX_PATH))
    {
        str=QString::fromWCharArray(buffer,-1);
        qDebug()<<"SysDir:"<<str;
        return true;
    }
    return false;
}

void getPicPath()
{
    QString SysDir = "";
    QString UserName = "";
    QString PicPath= "";
    static QString strAppend="AppData\\Local\\Packages\\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\\LocalState\\Assets";
    if(getWinPath(SysDir)&&getUserName(UserName))
    {
        QStringList list=SysDir.split(":");
        QString WinLetter=list[0];
        PicPath=WinLetter+":\\Users\\"+ UserName +"\\"+strAppend;
        qDebug()<<PicPath;
    }
    prefix=PicPath;
    qDebug()<<"getPicPath()|prefix=>"<<prefix;
    return;
}


