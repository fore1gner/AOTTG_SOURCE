using System.Collections;
using UnityEngine;

public class EffectLayer : MonoBehaviour
{
	public EffectNode[] ActiveENodes;

	public bool AlongVelocity;

	public int AngleAroundAxis;

	public bool AttractionAffectorEnable;

	public AnimationCurve AttractionCurve;

	public Vector3 AttractionPosition;

	public float AttractMag = 0.1f;

	public EffectNode[] AvailableENodes;

	public int AvailableNodeCount;

	public Vector3 BoxSize;

	public float ChanceToEmit = 100f;

	public Vector3 CircleDir;

	public Transform ClientTransform;

	public Color Color1 = Color.white;

	public Color Color2;

	public Color Color3;

	public Color Color4;

	public bool ColorAffectorEnable;

	public int ColorAffectType;

	public float ColorGradualTimeLength = 1f;

	public COLOR_GRADUAL_TYPE ColorGradualType;

	public int Cols = 1;

	public float DeltaRot;

	public float DeltaScaleX;

	public float DeltaScaleY;

	public float DiffDistance = 0.1f;

	public int EanIndex;

	public string EanPath = "none";

	public float EmitDelay;

	public float EmitDuration = 10f;

	public int EmitLoop = 1;

	public Vector3 EmitPoint;

	public int EmitRate = 20;

	protected Emitter emitter;

	public int EmitType;

	public bool IsEmitByDistance;

	public bool IsNodeLifeLoop = true;

	public bool IsRandomDir;

	public bool JetAffectorEnable;

	public float JetMax;

	public float JetMin;

	public Vector3 LastClientPos;

	public Vector3 LinearForce;

	public bool LinearForceAffectorEnable;

	public float LinearMagnitude = 1f;

	public float LineLengthLeft = -1f;

	public float LineLengthRight = 1f;

	public int LoopCircles = -1;

	protected Camera MainCamera;

	public Material Material;

	public int MaxENodes = 1;

	public float MaxFps = 60f;

	public int MaxRibbonElements = 6;

	public float NodeLifeMax = 1f;

	public float NodeLifeMin = 1f;

	public Vector2 OriLowerLeftUV = Vector2.zero;

	public int OriPoint;

	public int OriRotationMax;

	public int OriRotationMin;

	public float OriScaleXMax = 1f;

	public float OriScaleXMin = 1f;

	public float OriScaleYMax = 1f;

	public float OriScaleYMin = 1f;

	public float OriSpeed;

	public Vector2 OriUVDimensions = Vector2.one;

	public Vector3 OriVelocityAxis;

	public float Radius;

	public bool RandomOriRot;

	public bool RandomOriScale;

	public int RenderType;

	public float RibbonLen = 1f;

	public float RibbonWidth = 0.5f;

	public bool RotAffectorEnable;

	public AnimationCurve RotateCurve;

	public RSTYPE RotateType;

	public int Rows = 1;

	public bool ScaleAffectorEnable;

	public RSTYPE ScaleType;

	public AnimationCurve ScaleXCurve;

	public AnimationCurve ScaleYCurve;

	public float SpriteHeight = 1f;

	public int SpriteType;

	public int SpriteUVStretch;

	public float SpriteWidth = 1f;

	public float StartTime;

	public int StretchType;

	public bool SyncClient;

	public float TailDistance;

	public bool UseAttractCurve;

	public bool UseVortexCurve;

	public bool UVAffectorEnable;

	public float UVTime = 30f;

	public int UVType;

	public VertexPool Vertexpool;

	public bool VortexAffectorEnable;

	public AnimationCurve VortexCurve;

	public Vector3 VortexDirection;

	public float VortexMag = 0.1f;

	public void AddActiveNode(EffectNode node)
	{
		if (this.AvailableNodeCount == 0)
		{
			Debug.LogError("out index!");
		}
		if (this.AvailableENodes[node.Index] != null)
		{
			this.ActiveENodes[node.Index] = node;
			this.AvailableENodes[node.Index] = null;
			this.AvailableNodeCount--;
		}
	}

	protected void AddNodes(int num)
	{
		int num2 = 0;
		for (int i = 0; i < this.MaxENodes; i++)
		{
			if (num2 == num)
			{
				break;
			}
			EffectNode effectNode = this.AvailableENodes[i];
			if (effectNode != null)
			{
				this.AddActiveNode(effectNode);
				num2++;
				this.emitter.SetEmitPosition(effectNode);
				float num3 = 0f;
				effectNode.Init(life: (!this.IsNodeLifeLoop) ? Random.Range(this.NodeLifeMin, this.NodeLifeMax) : (-1f), oriDir: this.emitter.GetEmitRotation(effectNode).normalized, speed: this.OriSpeed, oriRot: Random.Range(this.OriRotationMin, this.OriRotationMax), oriScaleX: Random.Range(this.OriScaleXMin, this.OriScaleXMax), oriScaleY: Random.Range(this.OriScaleYMin, this.OriScaleYMax), oriColor: this.Color1, oriLowerUv: this.OriLowerLeftUV, oriUVDimension: this.OriUVDimensions);
			}
		}
	}

	public void FixedUpdateCustom()
	{
		int nodes = this.emitter.GetNodes();
		this.AddNodes(nodes);
		for (int i = 0; i < this.MaxENodes; i++)
		{
			this.ActiveENodes[i]?.Update();
		}
	}

	public RibbonTrail GetRibbonTrail()
	{
		if (!((this.ActiveENodes == null) | (this.ActiveENodes.Length != 1)) && this.MaxENodes == 1 && this.RenderType == 1)
		{
			return this.ActiveENodes[0].Ribbon;
		}
		return null;
	}

	public VertexPool GetVertexPool()
	{
		return this.Vertexpool;
	}

	protected void Init()
	{
		this.AvailableENodes = new EffectNode[this.MaxENodes];
		this.ActiveENodes = new EffectNode[this.MaxENodes];
		for (int i = 0; i < this.MaxENodes; i++)
		{
			EffectNode effectNode = new EffectNode(i, this.ClientTransform, this.SyncClient, this);
			ArrayList affectorList = this.InitAffectors(effectNode);
			effectNode.SetAffectorList(affectorList);
			if (this.RenderType == 0)
			{
				effectNode.SetType(this.SpriteWidth, this.SpriteHeight, (STYPE)this.SpriteType, (ORIPOINT)this.OriPoint, this.SpriteUVStretch, this.MaxFps);
			}
			else
			{
				effectNode.SetType(this.RibbonWidth, this.MaxRibbonElements, this.RibbonLen, this.ClientTransform.position, this.StretchType, this.MaxFps);
			}
			this.AvailableENodes[i] = effectNode;
		}
		this.AvailableNodeCount = this.MaxENodes;
		this.emitter = new Emitter(this);
	}

	protected ArrayList InitAffectors(EffectNode node)
	{
		ArrayList arrayList = new ArrayList();
		if (this.UVAffectorEnable)
		{
			UVAnimation uVAnimation = new UVAnimation();
			Texture texture = this.Vertexpool.GetMaterial().GetTexture("_MainTex");
			if (this.UVType == 2)
			{
				uVAnimation.BuildFromFile(this.EanPath, this.EanIndex, this.UVTime, texture);
				this.OriLowerLeftUV = uVAnimation.frames[0];
				this.OriUVDimensions = uVAnimation.UVDimensions[0];
			}
			else if (this.UVType == 1)
			{
				float num = texture.width / this.Cols;
				float num2 = texture.height / this.Rows;
				Vector2 vector = new Vector2(num / (float)texture.width, num2 / (float)texture.height);
				Vector2 vector2 = new Vector2(0f, 1f);
				uVAnimation.BuildUVAnim(vector2, vector, this.Cols, this.Rows, this.Cols * this.Rows);
				this.OriLowerLeftUV = vector2;
				this.OriUVDimensions = vector;
				this.OriUVDimensions.y = 0f - this.OriUVDimensions.y;
			}
			if (uVAnimation.frames.Length == 1)
			{
				this.OriLowerLeftUV = uVAnimation.frames[0];
				this.OriUVDimensions = uVAnimation.UVDimensions[0];
			}
			else
			{
				uVAnimation.loopCycles = this.LoopCircles;
				Affector value = new UVAffector(uVAnimation, this.UVTime, node);
				arrayList.Add(value);
			}
		}
		if (this.RotAffectorEnable && this.RotateType != 0)
		{
			Affector value2 = ((this.RotateType != RSTYPE.CURVE) ? new RotateAffector(this.DeltaRot, node) : new RotateAffector(this.RotateCurve, node));
			arrayList.Add(value2);
		}
		if (this.ScaleAffectorEnable && this.ScaleType != 0)
		{
			Affector value3 = ((this.ScaleType != RSTYPE.CURVE) ? new ScaleAffector(this.DeltaScaleX, this.DeltaScaleY, node) : new ScaleAffector(this.ScaleXCurve, this.ScaleYCurve, node));
			arrayList.Add(value3);
		}
		if (this.ColorAffectorEnable && this.ColorAffectType != 0)
		{
			ColorAffector value4 = ((this.ColorAffectType != 2) ? new ColorAffector(new Color[2] { this.Color1, this.Color2 }, this.ColorGradualTimeLength, this.ColorGradualType, node) : new ColorAffector(new Color[4] { this.Color1, this.Color2, this.Color3, this.Color4 }, this.ColorGradualTimeLength, this.ColorGradualType, node));
			arrayList.Add(value4);
		}
		if (this.LinearForceAffectorEnable)
		{
			Affector value5 = new LinearForceAffector(this.LinearForce.normalized * this.LinearMagnitude, node);
			arrayList.Add(value5);
		}
		if (this.JetAffectorEnable)
		{
			Affector value6 = new JetAffector(this.JetMin, this.JetMax, node);
			arrayList.Add(value6);
		}
		if (this.VortexAffectorEnable)
		{
			Affector value7 = ((!this.UseVortexCurve) ? new VortexAffector(this.VortexMag, this.VortexDirection, node) : new VortexAffector(this.VortexCurve, this.VortexDirection, node));
			arrayList.Add(value7);
		}
		if (this.AttractionAffectorEnable)
		{
			Affector value8 = ((!this.UseVortexCurve) ? new AttractionForceAffector(this.AttractMag, this.AttractionPosition, node) : new AttractionForceAffector(this.AttractionCurve, this.AttractionPosition, node));
			arrayList.Add(value8);
		}
		return arrayList;
	}

	private void OnDrawGizmosSelected()
	{
	}

	public void RemoveActiveNode(EffectNode node)
	{
		if (this.AvailableNodeCount == this.MaxENodes)
		{
			Debug.LogError("out index!");
		}
		if (this.ActiveENodes[node.Index] != null)
		{
			this.ActiveENodes[node.Index] = null;
			this.AvailableENodes[node.Index] = node;
			this.AvailableNodeCount++;
		}
	}

	public void Reset()
	{
		for (int i = 0; i < this.MaxENodes; i++)
		{
			if (this.ActiveENodes == null)
			{
				return;
			}
			EffectNode effectNode = this.ActiveENodes[i];
			if (effectNode != null)
			{
				effectNode.Reset();
				this.RemoveActiveNode(effectNode);
			}
		}
		this.emitter.Reset();
	}

	public void StartCustom()
	{
		if (this.MainCamera == null)
		{
			this.MainCamera = Camera.main;
		}
		this.Init();
		this.LastClientPos = this.ClientTransform.position;
	}
}
