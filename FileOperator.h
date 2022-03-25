#ifndef FILEOPERATOR_H
#define FILEOPERATOR_H

#include "commonHeaders.h"
#include <QTextStream>

bool isFolder(const QString& pathName);
bool isFile(const QString& pathName);
bool fileCopy(const QString _res, const QString _target);
bool copyPic(QString resFile, const QString& targetDir,QString num);
bool storeIni(const QStringList &fileNameList, QString& targetDir);
bool createFile(const QString& fileName);

#endif // FILEOPERATOR_H
