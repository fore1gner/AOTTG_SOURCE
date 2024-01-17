using UnityEngine;

public class Emitter
{
	private float EmitDelayTime;

	private float EmitLoop;

	private float EmitterElapsedTime;

	private bool IsFirstEmit = true;

	private Vector3 LastClientPos = Vector3.zero;

	public EffectLayer Layer;

	public Emitter(EffectLayer owner)
	{
		this.Layer = owner;
		this.EmitLoop = this.Layer.EmitLoop;
		this.LastClientPos = this.Layer.ClientTransform.position;
	}

	protected int EmitByDistance()
	{
		if ((this.Layer.ClientTransform.position - this.LastClientPos).magnitude >= this.Layer.DiffDistance)
		{
			this.LastClientPos = this.Layer.ClientTransform.position;
			return 1;
		}
		return 0;
	}

	protected int EmitByRate()
	{
		int num = Random.Range(0, 100);
		if (num >= 0 && (float)num > this.Layer.ChanceToEmit)
		{
			return 0;
		}
		this.EmitDelayTime += Time.deltaTime;
		if (this.EmitDelayTime < this.Layer.EmitDelay && !this.IsFirstEmit)
		{
			return 0;
		}
		this.EmitterElapsedTime += Time.deltaTime;
		if (this.EmitterElapsedTime >= this.Layer.EmitDuration)
		{
			if (this.EmitLoop > 0f)
			{
				this.EmitLoop -= 1f;
			}
			this.EmitterElapsedTime = 0f;
			this.EmitDelayTime = 0f;
			this.IsFirstEmit = false;
		}
		if (this.EmitLoop == 0f)
		{
			return 0;
		}
		if (this.Layer.AvailableNodeCount == 0)
		{
			return 0;
		}
		int num2 = (int)(this.EmitterElapsedTime * (float)this.Layer.EmitRate) - (this.Layer.ActiveENodes.Length - this.Layer.AvailableNodeCount);
		int num3 = 0;
		num3 = ((num2 <= this.Layer.AvailableNodeCount) ? num2 : this.Layer.AvailableNodeCount);
		if (num3 <= 0)
		{
			return 0;
		}
		return num3;
	}

	public Vector3 GetEmitRotation(EffectNode node)
	{
		_ = Vector3.zero;
		if (this.Layer.EmitType == 2)
		{
			if (!this.Layer.SyncClient)
			{
				return node.Position - (this.Layer.ClientTransform.position + this.Layer.EmitPoint);
			}
			return node.Position - this.Layer.EmitPoint;
		}
		if (this.Layer.EmitType == 3)
		{
			Vector3 vector = (this.Layer.SyncClient ? (node.Position - this.Layer.EmitPoint) : (node.Position - (this.Layer.ClientTransform.position + this.Layer.EmitPoint)));
			Vector3 toDirection = Vector3.RotateTowards(vector, this.Layer.CircleDir, (float)(90 - this.Layer.AngleAroundAxis) * 0.01745329f, 1f);
			return Quaternion.FromToRotation(vector, toDirection) * vector;
		}
		if (this.Layer.IsRandomDir)
		{
			Quaternion quaternion = Quaternion.Euler(0f, 0f, this.Layer.AngleAroundAxis);
			Quaternion quaternion2 = Quaternion.Euler(0f, Random.Range(0, 360), 0f);
			return Quaternion.FromToRotation(Vector3.up, this.Layer.OriVelocityAxis) * quaternion2 * quaternion * Vector3.up;
		}
		return this.Layer.OriVelocityAxis;
	}

	public int GetNodes()
	{
		if (this.Layer.IsEmitByDistance)
		{
			return this.EmitByDistance();
		}
		return this.EmitByRate();
	}

	public void Reset()
	{
		this.EmitterElapsedTime = 0f;
		this.EmitDelayTime = 0f;
		this.IsFirstEmit = true;
		this.EmitLoop = this.Layer.EmitLoop;
	}

	public void SetEmitPosition(EffectNode node)
	{
		Vector3 vector = Vector3.zero;
		if (this.Layer.EmitType == 1)
		{
			Vector3 emitPoint = this.Layer.EmitPoint;
			float x = Random.Range(emitPoint.x - this.Layer.BoxSize.x / 2f, emitPoint.x + this.Layer.BoxSize.x / 2f);
			float y = Random.Range(emitPoint.y - this.Layer.BoxSize.y / 2f, emitPoint.y + this.Layer.BoxSize.y / 2f);
			float z = Random.Range(emitPoint.z - this.Layer.BoxSize.z / 2f, emitPoint.z + this.Layer.BoxSize.z / 2f);
			vector.x = x;
			vector.y = y;
			vector.z = z;
			if (!this.Layer.SyncClient)
			{
				vector = this.Layer.ClientTransform.position + vector;
			}
		}
		else if (this.Layer.EmitType == 0)
		{
			vector = this.Layer.EmitPoint;
			if (!this.Layer.SyncClient)
			{
				vector = this.Layer.ClientTransform.position + this.Layer.EmitPoint;
			}
		}
		else if (this.Layer.EmitType == 2)
		{
			vector = this.Layer.EmitPoint;
			if (!this.Layer.SyncClient)
			{
				vector = this.Layer.ClientTransform.position + this.Layer.EmitPoint;
			}
			Vector3 vector2 = Vector3.up * this.Layer.Radius;
			vector = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)) * vector2 + vector;
		}
		else if (this.Layer.EmitType == 4)
		{
			Vector3 vector3 = this.Layer.EmitPoint + this.Layer.ClientTransform.localRotation * Vector3.forward * this.Layer.LineLengthLeft;
			Vector3 vector4 = this.Layer.EmitPoint + this.Layer.ClientTransform.localRotation * Vector3.forward * this.Layer.LineLengthRight - vector3;
			float num = (float)(node.Index + 1) / (float)this.Layer.MaxENodes;
			float num2 = vector4.magnitude * num;
			vector = vector3 + vector4.normalized * num2;
			if (!this.Layer.SyncClient)
			{
				vector = this.Layer.ClientTransform.TransformPoint(vector);
			}
		}
		else if (this.Layer.EmitType == 3)
		{
			float num3 = (float)(node.Index + 1) / (float)this.Layer.MaxENodes;
			float y2 = 360f * num3;
			Vector3 vector5 = Quaternion.Euler(0f, y2, 0f) * (Vector3.right * this.Layer.Radius);
			vector = Quaternion.FromToRotation(Vector3.up, this.Layer.CircleDir) * vector5;
			if (!this.Layer.SyncClient)
			{
				vector = this.Layer.ClientTransform.position + vector + this.Layer.EmitPoint;
			}
			else
			{
				vector += this.Layer.EmitPoint;
			}
		}
		node.SetLocalPosition(vector);
	}
}
