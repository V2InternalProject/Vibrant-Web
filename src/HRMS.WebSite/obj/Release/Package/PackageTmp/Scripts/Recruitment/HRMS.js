function aceResetPosition(object, args) {
    var tb = object._element; // tb.id is the associated textbox ID in the grid.
    var tbPos = $common.getLocation(tb);
    var xPos = tbPos.x;
    var yPos = tbPos.y;

    var ex = object._completionListElement;
    if (ex)

        $common.setLocation(ex, new Sys.UI.Point(xPos, yPos));
}