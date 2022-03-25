#ifndef ABOUTWINDOW_H
#define ABOUTWINDOW_H

#include <QWidget>
#include <commonHeaders.h>

namespace Ui {
class aboutWindow;
}

class aboutWindow : public QWidget
{
    Q_OBJECT

public:
    explicit aboutWindow(QWidget *parent = nullptr);
    ~aboutWindow();

private slots:
    void blog_linkActivated();

    void github_linkActivated();

private:
    Ui::aboutWindow *ui;
};

#endif // ABOUTWINDOW_H
