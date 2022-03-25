#include "mainwindow.h"
#include "aboutwindow.h"
#include "configwindow.h"
#include "ui_mainwindow.h"
#include "pathOperator.h"
#include "FileOperator.h"
#include "commonHeaders.h"
#include <QFileDialog>
#include <QDialog>
#include <QMovie>
#include <QLabel>
#include <QThread>
#include <QUrl>
#include <QPushButton>
#include <QHBoxLayout>
#include <QDesktopServices>

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    setWindowTitle("WinFocus");
    setWindowIcon(QIcon(":/icon.ico"));
    setFixedSize(320,180);

    this->qGif_pressed=new QMovie(":/myBtn_light.gif");
    this->qGif_pressed->setScaledSize(QSize(60,60));
    this->qGif_released=new QMovie(":/myBtn_dark.gif");
    this->qGif_released->setScaledSize(QSize(60,60));


    ui->apply_btn->setStyleSheet("border-image: url(:/myBtn.png)");
    ui->apply_btn->setWindowFlags(Qt::WindowStaysOnTopHint);
    ui->apply_lbl->close();


    btn_list={"border-image: url(:/btn/btn_1.png)","border-image: url(:/btn/btn_2.png)","border-image: url(:/btn/btn_3.png)",
              "border-image: url(:/btn/btn_3.png)","border-image: url(:/btn/btn_4.png)","border-image: url(:/btn/btn_6.png)",
              "border-image: url(:/btn/btn_5.png)","border-image: url(:/btn/btn_6.png)","border-image: url(:/btn/btn_9.png)"};

    QObject::connect(ui->actionAbout,&QAction::triggered,this,&MainWindow::about_clicked);
    QObject::connect(ui->actionConfig,&QAction::triggered,this,&MainWindow::config_clicked);


    cfg=new Config("Config.ini");
    loct=new QString("NULL");

    QVariant var=cfg->Get("Config","Location");
    if(var.isValid())
    {
        *loct = var.value<QString>();
        qDebug()<<*loct;
        QLabel* label =new QLabel(this);
        label->setFrameStyle(QFrame::Box|QFrame::Sunken);
        label->setStyleSheet("font-size:10px");
        label->setStyleSheet("color:black");
        label->setStyleSheet("font-style:italic");
        label->setText("保存路径已初始化");
        ui->statusbar->addPermanentWidget(label);
    }
    else
    {
        ui->statusbar->setStyleSheet("font-size:10px");
        ui->statusbar->setStyleSheet("color:red");
        ui->statusbar->showMessage(tr("保存路径未初始化"));
    }
}

MainWindow::~MainWindow()
{
    delete ui;
}


void MainWindow::updateLocatState()
{
    QVariant var=cfg->Get("Config","Location");
    if(var.isValid())
    {
        *loct = var.value<QString>();
        qDebug()<<*loct;
    }
}

void MainWindow::about_clicked()
{
    aboutWindow *aboutWin =new aboutWindow();
    qDebug()<<"about Menubar-menu clicked";
    aboutWin->show();
}


void MainWindow::config_clicked()
{
    configWindow *configWin =new configWindow();
    qDebug()<<"config Menubar-menu clicked";
    configWin->show();
}


void MainWindow::on_apply_btn_pressed()
{
    qDebug()<<"apply_Btn pressed";
    updateLocatState();
    if(*this->loct==QString::fromLocal8Bit("NULL"))
    {
        qDebug()<<"Location uninitialized.";
        ui->statusbar->setStyleSheet("font-size:10px");
        ui->statusbar->setStyleSheet("color:red");
        ui->statusbar->showMessage(tr("保存路径未初始化"));
        return;
    }
    ui->apply_btn->setEnabled(true);
    ui->openFolder->setEnabled(true);
    ui->apply_lbl->show();
    for(int i=0;i<=8;i++)
    {
        ui->apply_lbl->setStyleSheet(this->btn_list[i]);
        QThread::msleep(125);
    }

    getPicPath();
    QVector<QString> filenamelist;
    if(getPicName(prefix,filenamelist))
    {
        qDebug()<<"getPicNameList success!";
    }

    qDebug()<<"++++++++++++++++picNames+++++++++++++++++";

    for(int i=0;i<filenamelist.size();i++)
    {
        qDebug()<<filenamelist[i];
    }

    qDebug()<<"++++++++++++++++picNames+++++++++++++++++";

    ui->statusbar->setStyleSheet("font-size:11px;color:green");

    if(storeIni(filenamelist,*loct))
    {
        ui->statusbar->showMessage(tr("锁屏壁纸提取成功！"),200000);
        qDebug()<<"store FileName Success!";
        return;
    }

    ui->statusbar->showMessage(tr("锁屏壁纸提取失败！"),200000);
}


void MainWindow::on_apply_btn_released()
{
    qDebug()<<"pushBtn released";
    ui->apply_lbl->setMovie(this->qGif_released);
    ui->apply_lbl->show();
    this->qGif_released->start();
    ui->apply_lbl->setMovie(this->qGif_pressed);
    ui->apply_lbl->show();
    this->qGif_pressed->start();
}


void MainWindow::on_openFolder_clicked()
{
    qDebug()<<"openFolder_Btn pressed";
    updateLocatState();
    if(*this->loct==QString::fromLocal8Bit("NULL"))
    {
        qDebug()<<"Location uninitialized.";
        ui->statusbar->setStyleSheet("font-size:11px");
        ui->statusbar->setStyleSheet("color:red");
        ui->statusbar->showMessage(tr("保存路径未初始化"));
        return;
    }
    //Load FileStoreLocation
    qDebug()<<"Open Folder clicked";
    QDesktopServices::openUrl(QUrl::fromLocalFile(*loct));
}

