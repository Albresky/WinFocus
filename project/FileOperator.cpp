#include "FileOperator.h"
#include "commonHeaders.h"

bool isFolder(const QString& pathName)
{
    // File do not exist or FileName is Empty
    if (pathName.isEmpty() || !QDir().exists(pathName))
    {
        qDebug()<<"isEmpty OR do not exist => "<< pathName;
        return false;
    }

    QFileInfo FileInfo(pathName);

    if(FileInfo.isDir())
    {
        qDebug()<<"Folder Exist"<<" => "<< pathName;
        return true;
    }

    qDebug()<<"is not Folder"<<" => "<< pathName;
    return false;
}

bool isFile(const QString& pathName)
{
    // File do not exist or FileName is Empty
    if (pathName.isEmpty() || !QDir().exists(pathName))
    {
        qDebug()<<"isEmpty OR do not exist => "<< pathName;
        return false;
    }

    QFileInfo FileInfo(pathName);

    if(FileInfo.isFile())
    {
        qDebug()<<"File Exist"<<" => "<< pathName;
        return true;
    }

    qDebug()<<"is not File"<<" => "<< pathName;
    return false;
}


bool FileCopy(QString _res, QString _target)
{
    QFile qfile;
    if(qfile.copy(_res, _target))
    {
        qDebug()<<"FileCopy Success => "<< _target;
        return true;
    }
    else
    {
        qDebug()<<"FileCopy Fail => "<< _target;
        return false;
    }
}

bool copyPic(QString resFile,const QString& targetDir,QString num)
{
    if(!FileCopy(resFile,targetDir+"\\"+num+".jpg"))
    {
        qDebug()<<"pic copy OK!";
        return true;
    }
    qDebug()<<"pic copy Fail!";
    return false;
}

bool storeIni(const QStringList &fileNameList,QString& targetDir)
{
    QString IniName = "picname.ini";
    QFile file(IniName);
    if(!isFile(IniName))
    {
        file.open(QIODevice::ReadWrite|QIODevice::Text);
    }
    QVector<QString> existNames;
    QTextStream _ini(&file);
    while(!_ini.atEnd())
    {
        QString str=_ini.readLine();
        qDebug()<<str;
        existNames.append(str);
    }
    file.close();
    file.open(QIODevice::ReadWrite|QIODevice::Append|QIODevice::Text);
    for(int i=0;i<fileNameList.size();++i)
    {
        QString s=fileNameList[i];
        if(existNames.indexOf(s)==-1)
        {
            _ini<<s<<"\r\n";
            qDebug()<<"storeIni()|prefix=>"<<prefix;
            if(!copyPic(prefix+"\\"+s,targetDir,QString::number(i)))
            {
                file.close();
                return false;
            }
        }
    }
    file.close();
    return true;
}


bool createFile(const QString& fileName)
{
    if(isFile(fileName))
    {
        qDebug()<<"file exixts! skip create";
        return false;
    }
    QFile qfile(fileName);
    if(qfile.open(QIODevice::ReadWrite))
    {
        qDebug()<<"Create File Success => "<<fileName;
        qfile.close();
        return true;
    }
    qfile.close();
    return false;
}


