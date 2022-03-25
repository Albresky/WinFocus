#include "aboutwindow.h"
#include "ui_aboutwindow.h"
#include <QDesktopServices>
#include <QLatin1String>

aboutWindow::aboutWindow(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::aboutWindow)
{
    ui->setupUi(this);

    setWindowFlags(windowFlags()|Qt::Dialog);
    setWindowModality(Qt::ApplicationModal);
    setWindowFlags(windowFlags()&~Qt::WindowMinMaxButtonsHint);

    setWindowIcon(QIcon(":/icon.ico"));
    setWindowTitle("关于");
    setFixedSize(300,200);

    ui->name->setText(tr("<font style='font-size:18px;font-family:Microsoft YaHei;color:black'><b>WinFocus</b></font><font style='font-size:12px'>&nbsp;(x64)</font>"));

    ui->version->setText(tr("<font style='font-size:14px'>版本: 1.0</font>"));
    ui->builtdate->setText(tr("<font style='font-size:14px'>Built on Mar 22 2022</font>"));
    ui->author->setText(tr("<font style='font-size:14px'>Built by 代码之火</font>"));
    ui->blog->setText(tr("<font style='font-size:14px'>Blog@</font><font style='font-size:13px'><a href='url'>https://cosyspark.space</a></font><br><br>"));
    ui->github->setText(tr("<font style='font-size:14px'>Github@<font style='font-size:13px'><a href='url'>https://github.com/Albresky</a></font"));

    connect(ui->blog,&QLabel::linkActivated,this,&aboutWindow::blog_linkActivated);
    connect(ui->github,&QLabel::linkActivated,this,&aboutWindow::github_linkActivated);
}


aboutWindow::~aboutWindow()
{
    delete ui;
}


void aboutWindow::blog_linkActivated()
{
    QDesktopServices::openUrl(QUrl(QLatin1String("https://cosyspark.space")));
    qDebug()<<"blog hyperLink triggered";
}


void aboutWindow::github_linkActivated()
{
    QDesktopServices::openUrl(QUrl(QLatin1String("https://github.com/Albresky")));
    qDebug()<<"blog hyperLink triggered";
}

