#include "picOperator.h"
#include "FileOperator.h"
#include "commonHeaders.h"


bool getPicName(const QString &path, QStringList& fileNameList)
{
    if(!isFolder(path))
        return false;
    QDir dir(path);
    QStringList nameFilter;
    nameFilter.filter("*");
    fileNameList = dir.entryList(nameFilter,QDir::Files|QDir::Readable,QDir::Name);
    if(fileNameList.empty())
        return false;
    return true;
}
