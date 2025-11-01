using netDxf.aam.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace netDxf.aam.Utils
{
    /// <summary>
    /// Calc a bounding box
    /// </summary>
    public static class BoundingBox
    {
        /// <summary>
        /// Get the extends of an entity
        /// </summary>
        /// <param name="entityObject"></param>
        /// <returns></returns>
        public static Extends GetEntityExtends(IEntityObject entityObject)
        {
            switch (entityObject.Type)
            {
                case EntityType.Line:
                    Line line = entityObject as Line;
                    if (line != null)
                    {
                        Extends extends = new Extends(
                            new Vector3(
                                Math.Min(line.StartPoint.X, line.EndPoint.X), 
                                Math.Min(line.StartPoint.Y, line.EndPoint.Y), 
                                Math.Min(line.StartPoint.Z, line.EndPoint.Z)),
                            new Vector3(
                                Math.Max(line.StartPoint.X, line.EndPoint.X), 
                                Math.Max(line.StartPoint.Y, line.EndPoint.Y),
                                Math.Max(line.StartPoint.Z, line.EndPoint.Z))
                            );
                        extends.ExtendsBy(line.Thickness/2);
                        return extends;
                    }
                    break;
                case EntityType.Polyline:
                    Polyline polyline = entityObject as Polyline;
                    if (polyline != null)
                    {
                        Extends extends = new Extends();
                        foreach (var v in polyline.Vertexes)
                        {
                            extends.AddPoint(new Vector3(v.Location.X, v.Location.Y, 0));
                        }
                        extends.ExtendsBy(polyline.Thickness / 2);
                        return extends;
                    }
                    break;
                case EntityType.Polyline3d:
                    Polyline3d polyline3d = entityObject as Polyline3d;
                    if (polyline3d != null)
                    {
                        Extends extends = new Extends();
                        foreach (var v in polyline3d.Vertexes)
                        {
                            extends.AddPoint(v.Location);
                        }
                        return extends;
                    }
                    break;
                case EntityType.LightWeightPolyline:
                    LightWeightPolyline lightWeightPolyline = entityObject as LightWeightPolyline;
                    if (lightWeightPolyline != null)
                    {
                        Extends extends = new Extends();
                        foreach (var v in lightWeightPolyline.Vertexes)
                        {
                            extends.AddPoint(new Vector3(v.Location.X, v.Location.Y, 0));
                        }
                        extends.ExtendsBy(lightWeightPolyline.Thickness / 2);
                        return extends;
                    }
                    break;
                case EntityType.PolyfaceMesh:
                    PolyfaceMesh polyfaceMesh = entityObject as PolyfaceMesh;
                    if (polyfaceMesh != null)
                    {
                        Extends extends = new Extends();
                        foreach (var v in polyfaceMesh.Vertexes)
                        {
                            extends.AddPoint(v.Location);
                        }
                        return extends;
                    }
                    break;
                case EntityType.Circle:
                    Circle circle = entityObject as Circle;
                    if (circle != null)
                    {
                        Extends extends = new Extends(
                            new Vector3(
                                circle.Center.X - circle.Radius, 
                                circle.Center.Y - circle.Radius, 
                                circle.Center.Z - circle.Radius),
                            new Vector3(
                                circle.Center.X + circle.Radius, 
                                circle.Center.Y + circle.Radius, 
                                circle.Center.Z + circle.Radius)
                            );
                        extends.ExtendsBy(circle.Thickness / 2);
                        return extends;
                    }
                    break;
                case EntityType.NurbsCurve:
                    NurbsCurve nurbsCurve = entityObject as NurbsCurve;
                    if (nurbsCurve != null)
                    {
                        Extends extends = new Extends();
                        foreach (var v in nurbsCurve.ControlPoints)
                        {
                            extends.AddPoint(new Vector3(v.Location.X, v.Location.Y, 0));
                        }
                        return extends;
                    }
                    break;
                case EntityType.Ellipse:
                    Ellipse ellipse = entityObject as Ellipse;
                    if (ellipse != null)
                    {
                        Extends extends = new Extends(
                            new Vector3(
                                ellipse.Center.X - ellipse.MajorAxis,
                                ellipse.Center.Y - ellipse.MajorAxis,
                                ellipse.Center.Z - ellipse.MajorAxis),
                            new Vector3(
                                ellipse.Center.X + ellipse.MajorAxis,
                                ellipse.Center.Y + ellipse.MajorAxis,
                                ellipse.Center.Z + ellipse.MajorAxis)
                            );
                        extends.ExtendsBy(ellipse.Thickness / 2);
                        return extends;
                    }
                    break;
                case EntityType.Point:
                    Point point = entityObject as Point;
                    if (point != null)
                    {
                        Extends extends = new Extends(point.Location, point.Location);
                        extends.ExtendsBy(point.Thickness / 2);
                        return extends;
                    }
                    break;
                case EntityType.Arc:
                    Arc arc = entityObject as Arc;
                    if (arc != null)
                    {
                        Extends extends = new Extends(
                            new Vector3(
                                arc.Center.X - arc.Radius,
                                arc.Center.Y - arc.Radius,
                                arc.Center.Z - arc.Radius),
                            new Vector3(
                                arc.Center.X + arc.Radius,
                                arc.Center.Y + arc.Radius,
                                arc.Center.Z + arc.Radius)
                            );
                        extends.ExtendsBy(arc.Thickness / 2);
                        return extends;
                    }
                    break;
                case EntityType.Text:
                    Text text = entityObject as Text;
                    if (text != null)
                    {
                        Vector3 v1 = text.BasePoint;
                        Vector3 v2, v3, v4;
                        double textWidth = text.Height * text.WidthFactor * text.Value.Length;
                        v2 = new Vector3(v1.X, v1.Y + text.Height, 0);
                        v3 = new Vector3(v1.X + textWidth, v1.Y + text.Height, 0);
                        v4 = new Vector3(v1.X + textWidth, v1.Y, 0);

                        double angle = text.Rotation * MathHelper.DegToRad;
                        v2 = MathHelper.Rotation2d(v2, angle, v1);
                        v3 = MathHelper.Rotation2d(v3, angle, v1);
                        v4 = MathHelper.Rotation2d(v4, angle, v1);

                        Extends extends = new Extends();
                        extends.AddPoint(v1);
                        extends.AddPoint(v2);
                        extends.AddPoint(v3);
                        extends.AddPoint(v4);
                        return extends;
                    }
                    break;
                case EntityType.Face3D:
                    Face3d face3D = entityObject as Face3d;
                    if (face3D != null)
                    {
                        Extends extends = new Extends();
                        if ((face3D.EdgeFlags & EdgeFlags.First) != 0)
                            extends.AddPoint(face3D.FirstVertex);
                        if ((face3D.EdgeFlags & EdgeFlags.Second) != 0)
                            extends.AddPoint(face3D.SecondVertex);
                        if ((face3D.EdgeFlags & EdgeFlags.Third) != 0)
                            extends.AddPoint(face3D.ThirdVertex);
                        if ((face3D.EdgeFlags & EdgeFlags.Fourth) != 0)
                            extends.AddPoint(face3D.FourthVertex);

                        return extends;
                    }
                    break;
                case EntityType.Solid:
                    Solid solid = entityObject as Solid;
                    if (solid != null)
                    {
                        Extends extends = new Extends();
                        extends.AddPoint(solid.FirstVertex);
                        extends.AddPoint(solid.SecondVertex);
                        extends.AddPoint(solid.ThirdVertex);
                        extends.AddPoint(solid.FourthVertex);
                        extends.ExtendsBy(solid.Thickness);
                        return extends;
                    }
                    break;
                case EntityType.Insert:
                    Insert insert = entityObject as Insert;
                    if (insert != null)
                    {
                        Extends extends = new Extends();
                        insert.Block.Entities.ForEach(e =>
                        {
                            Extends ext = GetEntityExtends(e);
                            if (ext != null)
                            {
                                extends.AddExtends(ext);
                            }
                        });

                        extends.MinPoint = MathHelper.Translate2d(extends.MinPoint, insert.InsertionPoint);
                        extends.MaxPoint = MathHelper.Translate2d(extends.MaxPoint, insert.InsertionPoint);

                        Vector3 v1, v2, v3, v4;
                        v1 = new Vector3(extends.MinPoint.X, extends.MinPoint.Y, 0);
                        v2 = new Vector3(extends.MinPoint.X, extends.MaxPoint.Y, 0);
                        v3 = new Vector3(extends.MaxPoint.X, extends.MaxPoint.Y, 0);
                        v4 = new Vector3(extends.MaxPoint.X, extends.MinPoint.Y, 0);

                        double angle = insert.Rotation * MathHelper.DegToRad;

                        v1 = MathHelper.Scale2d(v1, insert.Scale, insert.InsertionPoint);
                        v1 = MathHelper.Rotation2d(v1, angle, insert.InsertionPoint);
                        v2 = MathHelper.Scale2d(v2, insert.Scale, insert.InsertionPoint);
                        v2 = MathHelper.Rotation2d(v2, angle, insert.InsertionPoint);
                        v3 = MathHelper.Scale2d(v3, insert.Scale, insert.InsertionPoint);
                        v3 = MathHelper.Rotation2d(v3, angle, insert.InsertionPoint);
                        v4 = MathHelper.Scale2d(v4, insert.Scale, insert.InsertionPoint);
                        v4 = MathHelper.Rotation2d(v4, angle, insert.InsertionPoint);

                        Extends extends1 = new Extends();
                        extends1.AddPoint(v1);
                        extends1.AddPoint(v2);
                        extends1.AddPoint(v3);
                        extends1.AddPoint(v4);
                        return extends1;
                    }
                    break;
                case EntityType.Hatch:
                    break;
                case EntityType.Attribute:
                    break;
                case EntityType.AttributeDefinition:
                    break;
                case EntityType.LightWeightPolylineVertex:
                    break;
                case EntityType.PolylineVertex:
                    PolylineVertex polylineVertex = entityObject as PolylineVertex;
                    if (polylineVertex != null)
                    {
                        Extends extends = new Extends(
                            new Vector3(polylineVertex.Location.X, polylineVertex.Location.Y, 0), 
                            new Vector3(polylineVertex.Location.X, polylineVertex.Location.Y, 0));
                        return extends;
                    }
                    break;
                case EntityType.Polyline3dVertex:
                    Polyline3dVertex polyline3DVertex = entityObject as Polyline3dVertex;
                    if (polyline3DVertex != null)
                    {
                        Extends extends = new Extends(
                            polyline3DVertex.Location,
                            polyline3DVertex.Location);
                        return extends;
                    }
                    break;
                case EntityType.PolyfaceMeshVertex:
                    PolyfaceMeshVertex polyfaceMeshVertex = entityObject as PolyfaceMeshVertex;
                    if (polyfaceMeshVertex != null)
                    {
                        Extends extends = new Extends(
                            polyfaceMeshVertex.Location,
                            polyfaceMeshVertex.Location);
                        return extends;
                    }
                    break;
                case EntityType.PolyfaceMeshFace:
                    break;
                case EntityType.Dimension:
                    break;
                case EntityType.Vertex:
                    Vertex vertex = entityObject as Vertex;
                    if (vertex != null)
                    {
                        Extends extends = new Extends(
                            vertex.Location,
                            vertex.Location);
                        return extends;
                    }
                    break;
                default:
                    break;
            }
            return new Extends();
        }
    }
        
}
