using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public enum UnitModel {None, X_Square, Y_Square, Z_Square, Cube};

public class SPLineModule : MonoBehaviour {

	
	public UnitModel unitModel = UnitModel.None;
	public float Scale = 1.0f;
	public Color LineColor = Color.yellow;
	public Vector3[] VertexPosition;
	
	private UnitModel NowUnitModel = UnitModel.None;
	
	// Render
	void OnDrawGizmos()
	{
		if( NowUnitModel != unitModel )
		{
			NowUnitModel = unitModel;
			if(NowUnitModel == UnitModel.X_Square)
			{
				VertexPosition = new Vector3[5];
				
				VertexPosition[0] = new Vector3(0.0f, 0.5f, 0.5f);
				VertexPosition[1] = new Vector3(0.0f, 0.5f, -0.5f);
				VertexPosition[2] = new Vector3(0.0f, -0.5f, -0.5f);
				VertexPosition[3] = new Vector3(0.0f, -0.5f, 0.5f);
				VertexPosition[4] = new Vector3(0.0f, 0.5f, 0.5f);
			}
			if(NowUnitModel == UnitModel.Y_Square)
			{
				VertexPosition = new Vector3[5];
				
				VertexPosition[0] = new Vector3(0.5f, 0.0f, 0.5f);
				VertexPosition[1] = new Vector3(0.5f, 0.0f, -0.5f);
				VertexPosition[2] = new Vector3(-0.5f, 0.0f, -0.5f);
				VertexPosition[3] = new Vector3(-0.5f, 0.0f, 0.5f);
				VertexPosition[4] = new Vector3(0.5f, 0.0f, 0.5f);
			}
			if(NowUnitModel == UnitModel.Z_Square)
			{
				VertexPosition = new Vector3[5];
				
				VertexPosition[0] = new Vector3(0.5f, 0.5f, 0.0f);
				VertexPosition[1] = new Vector3(0.5f, -0.5f, 0.0f);
				VertexPosition[2] = new Vector3(-0.5f, -0.5f, 0.0f);
				VertexPosition[3] = new Vector3(-0.5f, 0.5f, 0.0f);
				VertexPosition[4] = new Vector3(0.5f, 0.5f, 0.0f);
			}
			else if (NowUnitModel == UnitModel.Cube)
			{
				VertexPosition = new Vector3[30];
				VertexPosition[0] = new Vector3(0.5f, 0.5f, 0.5f);
				VertexPosition[1] = new Vector3(0.5f, 0.5f, -0.5f);
				VertexPosition[2] = new Vector3(0.5f, -0.5f, -0.5f);
				VertexPosition[3] = new Vector3(0.5f, -0.5f, 0.5f);
				VertexPosition[4] = new Vector3(0.5f, 0.5f, 0.5f);
				
				VertexPosition[5] = new Vector3(-0.5f, 0.5f, 0.5f);
				VertexPosition[6] = new Vector3(-0.5f, 0.5f, -0.5f);
				VertexPosition[7] = new Vector3(-0.5f, -0.5f, -0.5f);
				VertexPosition[8] = new Vector3(-0.5f, -0.5f, 0.5f);
				VertexPosition[9] = new Vector3(-0.5f, 0.5f, 0.5f);
				
				VertexPosition[10] = new Vector3(0.5f, 0.5f, 0.5f);
				VertexPosition[11] = new Vector3(0.5f, 0.5f, -0.5f);
				VertexPosition[12] = new Vector3(-0.5f, 0.5f, -0.5f);
				VertexPosition[13] = new Vector3(-0.5f, 0.5f, 0.5f);
				VertexPosition[14] = new Vector3(0.5f, 0.5f, 0.5f);
				
				VertexPosition[15] = new Vector3(0.5f, -0.5f, 0.5f);
				VertexPosition[16] = new Vector3(0.5f, -0.5f, -0.5f);
				VertexPosition[17] = new Vector3(-0.5f, -0.5f, -0.5f);
				VertexPosition[18] = new Vector3(-0.5f, -0.5f, 0.5f);
				VertexPosition[19] = new Vector3(0.5f, -0.5f, 0.5f);
				
				VertexPosition[20] = new Vector3(0.5f, 0.5f, 0.5f);
				VertexPosition[21] = new Vector3(0.5f, -0.5f, 0.5f);
				VertexPosition[22] = new Vector3(-0.5f, -0.5f, 0.5f);
				VertexPosition[23] = new Vector3(-0.5f, 0.5f, 0.5f);
				VertexPosition[24] = new Vector3(0.5f, 0.5f, 0.5f);
				
				VertexPosition[25] = new Vector3(0.5f, 0.5f, -0.5f);
				VertexPosition[26] = new Vector3(0.5f, -0.5f, -0.5f);
				VertexPosition[27] = new Vector3(-0.5f, -0.5f, -0.5f);
				VertexPosition[28] = new Vector3(-0.5f, 0.5f, -0.5f);
				VertexPosition[29] = new Vector3(0.5f, 0.5f, -0.5f);
				
			}
			else
			{
				// do nothing
			}				
			
		}
		
		if( VertexPosition != null )
		{
			if( VertexPosition.Length > 1 ) // 兩點以上才畫
			{
				Gizmos.color = LineColor;
				
				Vector3 start;
				Vector3 end;
				
				for( int i=0; i<VertexPosition.Length-1; i++ )
				{
					start = VertexPosition[i]*Scale;
					end = VertexPosition[i+1]*Scale;
					
					start.x *= transform.localScale.x;
					start.y *= transform.localScale.y;
					start.z *= transform.localScale.z;
					end.x *= transform.localScale.x;
					end.y *= transform.localScale.y;
					end.z *= transform.localScale.z;

					//start += transform.position;
					//end += transform.position;
					
					Vector3 rotation1 = transform.eulerAngles;
					Vector3 direction1 = start;
					Vector3 directionRotated = Quaternion.Euler(rotation1) * direction1;

					start = directionRotated + transform.position;
					

					Vector3 rotation2 = transform.eulerAngles;
					Vector3 direction2 = end;
					Vector3 directionRotated2 = Quaternion.Euler(rotation2) * direction2;
					
					end = directionRotated2 + transform.position;

					
					Gizmos.DrawLine(start, end);
				}
			}
		}
		
	}
}
