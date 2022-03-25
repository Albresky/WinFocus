#ifndef CONFIGWINDOW_H
#define CONFIGWINDOW_H

#include <QWidget>
#include <config.h>
#include <mainwindow.h>

namespace Ui {
class configWindow;
}

class configWindow : public QWidget
{
    Q_OBJECT

public:
    configWindow(QWidget *parent = nullptr);
    ~configWindow();

private slots:
    void on_langBox_activated(int index);

    void on_choosePath_clicked();

    void on_enter_clicked();

    void on_openFolder_clicked();

    void on_cancel_clicked();

private:
    Ui::configWindow *ui;
};

#endif // CONFIGWINDOW_H
