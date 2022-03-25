#include "configwindow.h"
#include "config.h"
#include "commonHeaders.h"
#include "ui_configwindow.h"
#include <QDesktopServices>

configWindow::configWindow(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::configWindow)
{
    ui->setupUi(this);

    setWindowFlags(windowFlags()|Qt::Dialog);
    setWindowModality(Qt::ApplicationModal);
    setWindowFlags(windowFlags()&~Qt::WindowMinMaxButtonsHint);

    setWindowIcon(QIcon(":/icon.ico"));
    setWindowTitle("配置");
    setFixedSize(370,160);

    setWindowFlags(windowFlags()&~Qt::WindowMinMaxButtonsHint);

    ui->lang->setText("界面语言");
    ui->outputPath->setText("图片保存位置");
    ui->pathLabel->setText("路径");
    ui->enter->setText("确定");
    ui->cancel->setText("取消");
    ui->openFolder->setText("打开文件路径");

    //Load Config File
    Config loadConfig=Config("Config.ini");

    //Load language
    QStringList langList={"zh-CN","English","Japanese"};
    for(int i=0;i<langList.size();++i)
    {
        if(langList[i]=="zh-CN")
        {
            ui->langBox->addItem("简体中文");
            continue;
        }
        ui->langBox->addItem(langList[i]);
    }

    QVariant lang = loadConfig.Get("Config","Language");
    QString langShow=lang.value<QString>();
    langShow =(langShow=="zh-CN")?"简体中文":langShow;
    ui->langBox->setCurrentText(langShow);


    //    if(ui->langBox->findText(langShow)==-1)
    //        ui->langBox->addItem("English");
    qDebug()<<"load language => "<<langShow;

    //Load FileStoreLocation
    QVariant location=loadConfig.Get("Config","Location");
    ui->locationLabel->setText(location.value<QString>());
    ui->locationLabel->setStyleSheet("Font-size:14px");
    qDebug()<<"load picture store path => "<<location;

}

configWindow::~configWindow()
{
    delete ui;
}

void configWindow::on_langBox_activated(int index)
{

}



void configWindow::on_choosePath_clicked()
{
    QString ManualSelectDir =
            QDir::toNativeSeparators(QFileDialog::getExistingDirectory(this,tr("Save Path"), QDir::currentPath()));
    if(!ManualSelectDir.isEmpty())
    {
        ui->locationLabel->setText(ManualSelectDir);
        ui->locationLabel->setStyleSheet("Font-size:8px");
        ui->locationLabel->setStyleSheet("font-style:italic");
    }
}


void configWindow::on_enter_clicked()
{
    Config loadConfig=Config("Config.ini");
    loadConfig.Set("Config","Language",QVariant(ui->langBox->currentText()));
    loadConfig.Set("Config","Location",QVariant(ui->locationLabel->text()));
    this->close();
}


void configWindow::on_openFolder_clicked()
{
    QDesktopServices::openUrl(QUrl::fromLocalFile(ui->locationLabel->text()));
}


void configWindow::on_cancel_clicked()
{
    this->close();
}

