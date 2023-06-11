using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using System.Collections.Generic;
using System.Windows;


namespace RP_test
{
    /// <summary>
    /// Логика взаимодействия для MainForm.xaml
    /// </summary>
    public partial class MainForm : Window
    {
        Document doc;
        ICollection<Element> rooms;
        FilteredElementCollector roomsTagTypes;
        public MainForm(Document Doc)
        {
            InitializeComponent();
            doc = Doc;
            rooms = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Rooms).WhereElementIsNotElementType().ToElements();
            roomsTagTypes = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_RoomTags).WhereElementIsElementType();
            DG.ItemsSource = rooms;
            ComboCenter.ItemsSource = roomsTagTypes;
            ComboCorner.ItemsSource = roomsTagTypes;
           // DG.ItemsSource = roomsTagTypes;
       
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Transaction transaction = new Transaction(doc);
            transaction.Start("SetMarks");

            View v = doc.ActiveView;

            int centerTag = ComboCenter.SelectedIndex;
            int cornerTag = ComboCorner.SelectedIndex;


            foreach (Element roomEl in rooms)
            {
                Room room = roomEl as Room;
                XYZ center = GetRoomCenter(room);
                XYZ corner = GetRoomCorner(room);
                var tag1 = doc.Create.NewRoomTag(new LinkElementId(roomEl.Id), new UV(center.X, center.Y), v.Id);
                var tag2 = doc.Create.NewRoomTag(new LinkElementId(roomEl.Id), new UV(corner.X, corner.Y), v.Id);

                Element newtype1 = roomsTagTypes.ToElements()[centerTag];
                tag1.ChangeTypeId(newtype1.Id);

                Element newtype2 = roomsTagTypes.ToElements()[cornerTag];
                tag2.ChangeTypeId(newtype2.Id);
            }

            transaction.Commit();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Transaction transaction = new Transaction(doc);
            transaction.Start("SetMarks");

            View v = doc.ActiveView;

            var selectedList = DG.SelectedItems;            

            foreach (Element roomEl in selectedList)
            {
                Room room = roomEl as Room;
                XYZ center = GetRoomCenter(room);
                XYZ corner = GetRoomCorner(room);
                var tag1 = doc.Create.NewRoomTag(new LinkElementId(roomEl.Id), new UV(center.X, center.Y), v.Id);
                var tag2 = doc.Create.NewRoomTag(new LinkElementId(roomEl.Id), new UV(corner.X, corner.Y), v.Id);

                Element newtype1 = roomsTagTypes.ToElements()[15];
                tag1.ChangeTypeId(newtype1.Id);

                Element newtype2 = roomsTagTypes.ToElements()[17];
                tag2.ChangeTypeId(newtype2.Id);
            }

            transaction.Commit();
        }

        public XYZ GetRoomCenter(Room room)
        {
            XYZ boundCenter = GetElementCenter(room);
            LocationPoint locPt = (LocationPoint)room.Location;
            XYZ roomCenter = new XYZ(boundCenter.X, boundCenter.Y, locPt.Point.Z);
            return roomCenter;
        }
        public XYZ GetElementCenter(Element elem)
        {
            BoundingBoxXYZ bounding = elem.get_BoundingBox(null);
            XYZ center = (bounding.Max + bounding.Min) * 0.5;
            return center;
        }

        public XYZ GetRoomCorner(Room room)
        {
            XYZ boundCorner = GetElementCorner(room, 0.1);
            LocationPoint locPt = (LocationPoint)room.Location;
            XYZ roomCorner = new XYZ(boundCorner.X, boundCorner.Y, locPt.Point.Z);
            return roomCorner;
        }
        public XYZ GetElementCorner(Element elem, double shift)
        {
            BoundingBoxXYZ bounding = elem.get_BoundingBox(null);
            XYZ Corner = new XYZ(bounding.Max.X - (shift * (bounding.Max.X - bounding.Min.X)), bounding.Min.Y - (shift * (bounding.Min.Y - bounding.Max.Y)), (bounding.Max.Z + bounding.Min.Z) * 0.5);
            return Corner;
        }


    }
}
