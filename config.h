#ifndef CONFIG_H
#define CONFIG_H

#include <QVariant>
#include <QSettings>
#include <QCoreApplication>

class Config
{
public:
    Config(QString cfgFile="");
    virtual ~Config(void);
    void Set(QString,QString,QVariant);
    QVariant Get(QString,QString);
private:
    QString pri_cfgFile;
    QSettings *pri_pSettings;
};

#endif // CONFIG_H
