#include "pathOperator.h"
#include "commonHeaders.h"

bool getUserName(QString *str)
{
    const int MAX_LENGTH=256;
    wchar_t buffer[MAX_LENGTH];
    DWORD len=MAX_LENGTH;
    if(GetUserName(buffer, &len))
    {
        *str=QString::fromWCharArray(buffer,-1);
        qDebug()<<"Username:"<<*str;
        return true;
    }
    return false;
}

bool getWinPath(QString* str)
{
    const int MAX_LENGTH=256;
    wchar_t buffer[MAX_LENGTH];
    if(GetWindowsDirectory(buffer, MAX_PATH))
    {
        *str=QString::fromWCharArray(buffer,-1);
        qDebug()<<"SysDir:"<<*str;
        return true;
    }
    return false;
}

void getPicPath()
{
    QString *SysDir = new QString;
    QString *UserName = new QString;
    QString *PicPath= new QString;
    static QString strAppend="AppData\\Local\\Packages\\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\\LocalState\\Assets";
    if(getWinPath(SysDir)&&getUserName(UserName))
    {
        QStringList list=SysDir->split(":");
        QString WinLetter=list[0];
        *PicPath=WinLetter+":\\Users\\"+*UserName+"\\"+strAppend;
        qDebug()<<*PicPath;
    }
    delete SysDir;
    delete UserName;
    prefix=*PicPath;
    qDebug()<<"getPicPath()|prefix=>"<<prefix;
    return;
}


